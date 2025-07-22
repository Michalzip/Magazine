using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Exceptions;
using Domain.IServices;

namespace Infrastructure.Services
{
    // Serwis do czytania plików CSV i mapowania ich na obiekty
    public class CsvService : ICsvService
    {
        // Odczytuje plik CSV i mapuje rekordy na obiekty typu T w celu zwiekszenia uniwersalnosci
        public IEnumerable<T> ReadCsv<T>(
            string filePath,
            string seperator,
            bool haveHeader,
            CsvConfiguration? config = null
        )
        {
            // Konfiguracja CSV (domyślna lub przekazana)
            config ??= new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = seperator,
                HasHeaderRecord = haveHeader,
                MissingFieldFound = null, // Ignoruj brakujące pola
                BadDataFound = null // Ignoruj błędne dane
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            var records = new List<T>();

            while (csv.Read())
            {
                try
                {
                    // Mapowanie rekordu na obiekt typu T
                    var record = csv.GetRecord<T>();
                    records.Add(record);
                }
                // Obsługa błędów mapowania i czytania CSV
                catch (Exception ex)
                {
                    throw new CsvImportException(
                        $"Błąd podczas czytania pliku CSV:  {csv.Context}: {ex.Message}"
                    );
                }
            }

            return records;
        }
    }
}
