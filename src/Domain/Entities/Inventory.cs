using CsvHelper.Configuration.Attributes;
using Magazine.Domain.Converters;

namespace Domain
{
    [Microsoft.EntityFrameworkCore.Index(nameof(Sku))]
    public class Inventory
    {
        [Ignore]
        public int Id { get; set; }

        [Name("product_id")]
        public string ProductId { get; set; }

        [Name("sku")]
        public string Sku { get; set; } = string.Empty;

        [Name("unit")]
        public string Unit { get; set; } = string.Empty;

        [Name("qty")]
        [TypeConverter(typeof(IntConverter))]
        public int Quantity { get; set; }

        [Name("manufacturer_name")]
        public string Manufacturer { get; set; } = string.Empty;

        [Name("shipping")]
        public string Shipping { get; set; }

        [Name("shipping_cost")]
        [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DecimalConverter))]
        public decimal? ShippingCost { get; set; }
    }
}
