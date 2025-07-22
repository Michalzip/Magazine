using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Magazine.Domain.Converters;

public class IntConverter : DefaultTypeConverter
{
    public override object ConvertFromString(
        string? text,
        IReaderRow row,
        MemberMapData memberMapData
    )
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        var digits = new string(text.Where(char.IsDigit).ToArray());

        return int.TryParse(digits, out var result) ? result : 0;
    }
}
