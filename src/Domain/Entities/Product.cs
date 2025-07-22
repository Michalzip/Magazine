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
        //zrobilem wlasne id zeby nie bylo przypadku gdy dwa systemy serwisow ktore integrujemy wygeneowaly te same InteralProductId, mała szansa ale zawsze jakas jest.
        public int Id { get; set; }

        [Name("ID")]
        public string InteralProductId { get; set; }

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

        [Name("is_wire")]
        [TypeConverter(typeof(BoolFromZeroOneConverter))]
        public bool IsWire { get; set; }

        [Name("shipping")]
        [TypeConverter(typeof(IntConverter))]
        public int Shipping { get; set; }

        [Name("available")]
        [TypeConverter(typeof(BoolFromZeroOneConverter))]
        public bool? Available { get; set; }

        [Name("is_vendor")]
        [TypeConverter(typeof(BoolFromZeroOneConverter))]
        public bool? IsVendor { get; set; }

        [Name("default_image")]
        public string? DefaultImage { get; set; } = string.Empty;
    }
}
