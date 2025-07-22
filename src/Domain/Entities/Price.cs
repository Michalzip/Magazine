using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using DecimalConverter = Magazine.Domain.Converters.DecimalConverter;

namespace Domain
{
    [Microsoft.EntityFrameworkCore.Index(nameof(Sku))]
    public class Price
    {
        [Ignore]
        [Key]
        public int Id { get; set; }

        [Index(1)]
        public string Sku { get; set; } = string.Empty;

        [Index(5)]
        [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DecimalConverter))]
        public decimal? NetUnitPriceAfterDiscount { get; set; }
    }
}
