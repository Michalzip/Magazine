using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Magazine.Domain.Converters;

namespace Domain
{
    [Microsoft.EntityFrameworkCore.Index(nameof(Sku))]
    public class Product
    {
        [Ignore]
        [Key]
        public int Id { get; set; }

        [Name("SKU")]
        public string Sku { get; set; } = string.Empty;

        [Name("name")]
        public string Name { get; set; } = string.Empty;

        [Name("EAN")]
        public string Ean { get; set; } = string.Empty;

        [Name("producer_name")]
        public string ProducerName { get; set; } = string.Empty;

        [Name("category")]
        public string Category { get; set; } = string.Empty;

        [Name("shipping")]
        [TypeConverter(typeof(IntConverter))]
        public int Shipping { get; set; }

        [Name("is_wire")]
        [NotMapped]
        [TypeConverter(typeof(BoolFromZeroOneConverter))]
        public bool? IsWire { get; set; }
    }
}
