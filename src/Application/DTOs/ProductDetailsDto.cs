// DTO (Data Transfer Object) reprezentujący szczegóły produktu zwracane przez API
namespace Application.DTOs
{
    public class ProductDetailsDto
    {
        public string Name { get; set; } = string.Empty;
        public string Ean { get; set; } = string.Empty;
        public string ProducerName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string DefaultImage { get; set; } = string.Empty;

        // z Inventory
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal? ShippingCost { get; set; }

        // Cena netto po obnizkach
        public decimal NetUnitPrice { get; set; }
    }
}
