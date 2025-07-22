// Zapytanie CQRS pobierające szczegóły produktu na podstawie SKU
using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetProductDetailsQuery : IRequest<ProductDetailsDto>
    {
        // SKU produktu, dla którego pobierane są szczegóły
        public string Sku { get; }

        // Konstruktor przyjmujący SKU produktu
        public GetProductDetailsQuery(string sku)
        {
            Sku = sku;
        }
    }
}
