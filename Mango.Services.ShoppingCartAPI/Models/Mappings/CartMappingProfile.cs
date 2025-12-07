using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Models.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            this.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            this.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        }
    }
}
