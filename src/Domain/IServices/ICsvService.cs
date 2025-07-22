using CsvHelper.Configuration;

namespace Domain.IServices;

public interface ICsvService
{
    IEnumerable<T> ReadCsv<T>(
        string filePath,
        string seperator,
        bool haveHeader,
        CsvConfiguration? config = null
    );
}
