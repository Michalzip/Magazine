using Domain.Exceptions;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;

// Serwis do zapisu plików na dysku z logowaniem i obsługą wyjątków
public class FileStorageService : IFileStorageService
{
    // Logger do rejestrowania operacji zapisu
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(ILogger<FileStorageService> logger)
    {
        _logger = logger;
    }

    // Zapisuje plik na dysku, zwraca ścieżkę do pliku
    public async Task<string> SaveFileAsync(string fileName, Stream fileStream)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

        _logger.LogInformation("Zapis pliku: {Path}", filePath);

        try
        {
            // Tworzy plik i kopiuje zawartość ze strumienia (chunk 4MB aby lepiej obsłużyć duże pliki)
            using (var file = File.Create(filePath))
            {
                await fileStream.CopyToAsync(file, 4 * 1024 * 1024);
            }
        }
        // Obsługa błędów zapisu pliku
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas zapisu pliku: {Path}", filePath);
            throw new FileStorageException($"Błąd podczas zapisu pliku: {ex.Message}");
        }

        _logger.LogInformation("Zapisano plik w: {Path}", filePath);
        return filePath;
    }
}
