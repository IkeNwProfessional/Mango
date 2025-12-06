using AutoMapper;

namespace Mango.Services.ProductAPI.Models.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            this.CreateMap<Models.Dto.ProductDto, Models.Product>();
            this.CreateMap<Models.Product, Models.Dto.ProductDto>();
        }
    }
}
