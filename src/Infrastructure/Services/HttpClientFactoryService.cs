using Domain.Exceptions;

namespace Infrastructure.Http
{
    // Serwis do pobierania plików przez HTTP z obsługą retry i logowaniem
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        // Fabryka klienta HTTP (wstrzykiwana przez DI)
        private readonly System.Net.Http.IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpClientFactoryService> _logger;

        public HttpClientFactoryService(
            System.Net.Http.IHttpClientFactory httpClientFactory,
            ILogger<HttpClientFactoryService> logger
        )
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // Pobiera plik z podanego URL z obsługą retry dla slabszego internetu.
        public async Task<Stream> DownloadAsync(string url)
        {
            var client = _httpClientFactory.CreateClient("CsvClient");

            const int maxRetries = 3; // Maksymalna liczba prób
            int delayMs = 2000; // Początkowe opóźnienie między próbami (ms)

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logger.LogInformation(
                        "Pobieranie CSV, próba {Attempt} z {MaxRetries}...",
                        attempt,
                        maxRetries
                    );

                    // Pobranie pliku z serwera (stream)
                    var response = await client.GetAsync(
                        url,
                        HttpCompletionOption.ResponseHeadersRead
                    );

                    response.EnsureSuccessStatusCode(); // Rzuca wyjątek jeśli status != 2xx

                    _logger.LogInformation("Plik CSV został pobrany pomyślnie.");

                    return await response.Content.ReadAsStreamAsync();
                }
                // Obsługa błędów HTTP z retry
                catch (HttpRequestException ex) when (attempt < maxRetries)
                {
                    _logger.LogWarning(
                        "Błąd HTTP przy pobieraniu CSV: {Message}. Ponawiam próbę za {Delay} ms...",
                        ex.Message,
                        delayMs
                    );
                    await Task.Delay(delayMs);
                    delayMs *= 2; // Exponential backoff
                }
                // Obsługa timeoutów z retry
                catch (TaskCanceledException ex) when (attempt < maxRetries)
                {
                    _logger.LogWarning(
                        "Timeout przy pobieraniu CSV (próba {Attempt}): {Message}. Ponawiam...",
                        attempt,
                        ex.Message
                    );
                    await Task.Delay(delayMs);
                    delayMs *= 2;
                }
                // Krytyczne błędy - nie ponawiamy
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Krytyczny błąd przy pobieraniu CSV: {Message}",
                        ex.Message
                    );
                    throw new HttpDownloadException(
                        $"Nie udało się pobrać pliku CSV. {ex.Message}."
                    );
                }
            }

            // Jeśli nie udało się pobrać po wszystkich próbach
            throw new Exception("Przekroczono maksymalną liczbę prób pobrania pliku CSV.");
        }
    }
}
