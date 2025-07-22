using CsvHelper.Configuration.Attributes;
using Magazine.Domain.Converters;

namespace Domain
{
    [Microsoft.EntityFrameworkCore.Index(nameof(Sku))]
    public class Inventory
    {
        [Ignore]
        public int Id { get; set; }

        [Name("sku")]
        public string Sku { get; set; } = string.Empty;

        [Name("unit")]
        public string Unit { get; set; } = string.Empty;

        [Name("qty")]
        [TypeConverter(typeof(IntConverter))]
        public int Quantity { get; set; }

        [Name("shipping_cost")]
        [TypeConverter(typeof(DecimalConverter))]
        public decimal? ShippingCost { get; set; }

        [Name("shipping")]
        public string Shipping { get; set; }
    }
}
