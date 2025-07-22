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
      
        public int Quantity { get; set; }
      
        public string Unit { get; set; } = string.Empty;
        
        public decimal NetPrice { get; set; }
        
        public decimal? ShippingCost { get; set; }
    }
}
