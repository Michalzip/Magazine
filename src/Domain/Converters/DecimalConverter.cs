using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Magazine.Domain.Converters;

public class DecimalConverter : DefaultTypeConverter
{
    public override object ConvertFromString(
        string? text,
        IReaderRow row,
        MemberMapData memberMapData
    )
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return 0m;
        }

        text = text.Trim().Replace(',', '.');

        if (
            decimal.TryParse(
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out decimal result
            )
        )
        {
            return result;
        }

        return 0m;
    }
}
