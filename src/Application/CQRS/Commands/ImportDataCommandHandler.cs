using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;
using Domain.IServices;
using Infrastructure.Http;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    // Handler obsługujący komendę importu danych z plików CSV do bazy danych
    public class ImportDataCommandHandler : IRequestHandler<ImportDataCommand, Unit>
    {
        // Repozytoria do zapisu danych w bazie
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Inventory> _inventoryRepository;
        private readonly IRepository<Price> _priceRepository;
        // Serwis do czytania plików CSV
        private readonly ICsvService _csvService;
        // Konfiguracja aplikacji (np. URL-e plików CSV)
        private readonly IConfiguration _configuration;
        // Serwis do pobierania plików przez HTTP
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        // Serwis do zapisu plików na dysku
        private readonly IFileStorageService _fileStorageService;
        // Logger do rejestrowania przebiegu importu
        private readonly ILogger<ImportDataCommandHandler> _logger;

        // Konstruktor z wstrzykiwaniem zależności (DI)
        public ImportDataCommandHandler(
            IRepository<Product> productRepository,
            IRepository<Inventory> inventoryRepository,
            IRepository<Price> priceRepository,
            ICsvService csvService,
            IConfiguration configuration,
            IHttpClientFactoryService httpClientFactoryService,
            IFileStorageService fileStorageService,
            ILogger<ImportDataCommandHandler> logger
        )
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _priceRepository = priceRepository;
            _csvService = csvService;
            _configuration = configuration;
            _httpClientFactoryService = httpClientFactoryService;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        // Warto zastosować wzorzec fabryki, gdyby trzeba było obsłużyć więcej plików CSV lub różne formaty.
        public async Task<Unit> Handle(
            ImportDataCommand request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation("Rozpoczynam import danych z CSV.");

            // Import produktów, stanów magazynowych i cen w osobnych krokach
            await ImportProductsAsync();
            await ImportInventoryAsync();
            await ImportPricesAsync();

            _logger.LogInformation("Import danych CSV zakończony pomyślnie.");

            return Unit.Value;
        }

        // Importuje produkty z pliku CSV, filtruje i zapisuje do bazy
        private async Task ImportProductsAsync()
        {
            var productsPath = Path.Combine(AppContext.BaseDirectory, "Products.csv");
            _logger.LogInformation("Pobieram plik Products.csv z URL.");
            var productsUrl = _configuration["CsvUrls:Products"]!;
            var productsStream = await _httpClientFactoryService.DownloadAsync(productsUrl);
            await _fileStorageService.SaveFileAsync("Products.csv", productsStream);
            _logger.LogInformation("Zapisano plik Products.csv.");

            // Wczytanie wszystkich produktów z pliku CSV
            var allProducts = _csvService.ReadCsv<Product>(productsPath, ";", true);
            // Filtrowanie: tylko produkty, które NIE są przewodami i mają czas wysyłki <= 24h
            var filteredProducts = allProducts.Where(p => !p.IsWire && p.Shipping <= 24).ToList();

            _logger.LogInformation(
                "Wczytano {Count} produktów (po filtrze).",
                filteredProducts.Count
            );
            // Zapis przefiltrowanych produktów do bazy
            await _productRepository.BulkInsertAsync(filteredProducts);
            _logger.LogInformation("Zapisano produkty do bazy danych.");
        }

        // Importuje stany magazynowe, filtruje po produktach z szybkim czasem wysyłki
        private async Task ImportInventoryAsync()
        {
            var inventoryPath = Path.Combine(AppContext.BaseDirectory, "Inventory.csv");

            _logger.LogInformation("Pobieram plik Inventory.csv z URL.");
            var inventoryUrl = _configuration["CsvUrls:Inventory"]!;
            var inventoryStream = await _httpClientFactoryService.DownloadAsync(inventoryUrl);
            await _fileStorageService.SaveFileAsync("Inventory.csv", inventoryStream);
            _logger.LogInformation("Zapisano plik Inventory.csv.");

            // Wczytanie produktów, aby znać listę dozwolonych ID
            var productsPath = Path.Combine(AppContext.BaseDirectory, "Products.csv");
            var allProducts = _csvService.ReadCsv<Product>(productsPath, ";", true);
            var allInventories = _csvService.ReadCsv<Inventory>(inventoryPath, ",", true);
            // Filtrowanie: tylko stany magazynowe dla produktów z czasem wysyłki <= 24h
            var filteredProductIds = allProducts
                .Where(p => p.Shipping <= 24)
                .Select(x => x.InteralProductId)
                .ToHashSet();

            var filteredInventories = allInventories
                .Where(inv => filteredProductIds.Contains(inv.ProductId))
                .ToList();

            _logger.LogInformation(
                "Wczytano {Count} stanów magazynowych (po filtrze).",
                filteredInventories.Count
            );
            // Zapis przefiltrowanych stanów magazynowych do bazy
            await _inventoryRepository.BulkInsertAsync(filteredInventories);
            _logger.LogInformation("Zapisano stany magazynowe do bazy danych.");
        }

        // Importuje ceny produktów z pliku CSV i zapisuje do bazy
        private async Task ImportPricesAsync()
        {
            var pricesPath = Path.Combine(AppContext.BaseDirectory, "Prices.csv");
            _logger.LogInformation("Pobieram plik Prices.csv z URL.");
            var pricesUrl = _configuration["CsvUrls:Prices"]!;
            var pricesStream = await _httpClientFactoryService.DownloadAsync(pricesUrl);
            await _fileStorageService.SaveFileAsync("Prices.csv", pricesStream);
            _logger.LogInformation("Zapisano plik Prices.csv.");

            // Wczytanie wszystkich cen z pliku CSV
            var allPrices = _csvService.ReadCsv<Price>(pricesPath, ",", false);
            _logger.LogInformation("Wczytano {Count} cen.", allPrices.Count());
            // Zapis cen do bazy
            await _priceRepository.BulkInsertAsync(allPrices);
            _logger.LogInformation("Zapisano ceny do bazy danych.");
        }
    }
}
