using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Magazine.Domain.Converters;

public class BoolFromZeroOneConverter : DefaultTypeConverter
{
    public override object ConvertFromString(
        string? text,
        IReaderRow row,
        MemberMapData memberMapData
    )
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        text = text?.Trim();

        return text switch
        {
            "1" => true,
            "0" => false,
            _ => false
        };
    }
}
