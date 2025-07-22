// Profil AutoMapper definiujący mapowanie z encji Product na DTO ProductDetailsDto
using AutoMapper;
using Domain;

namespace Application.DTOs
{
    public class ProductDetailsDtoProfile : Profile
    {
        public ProductDetailsDtoProfile()
        {
            // Mapowanie właściwości z Product na ProductDetailsDto
            CreateMap<Product, ProductDetailsDto>();
        }
    }
}
