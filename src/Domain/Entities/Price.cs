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
        //zrobilem wlasne id zeby nie bylo przypadku gdy dwa systemy serwisow ktore integrujemy wygeneowaly te same id
        public int Id { get; set; }

        [Index(0)]
        public string InternalId { get; set; }

        [Index(1)]
        public string Sku { get; set; } = string.Empty;

        [Index(2)]
        [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DecimalConverter))]
        public decimal NetPrice { get; set; }

        [Index(3)]
        [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DecimalConverter))]
        public decimal NetPriceAfterDiscount { get; set; }

        [Index(4)]
        [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DecimalConverter))]
        public decimal? VatRate { get; set; }

        [Index(5)]
        [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DecimalConverter))]
        public decimal? NetUnitPriceAfterDiscount { get; set; }
    }
}
