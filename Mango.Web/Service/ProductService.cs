using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService baseService;

        public ProductService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Data = productDto,
                Url = StaticData.ProductAPIBase + "/api/productapi/createproduct"
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.DELETE,
                Url = StaticData.ProductAPIBase + "/api/productapi/deleteproduct/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.ProductAPIBase + "/api/productapi/getallproducts"
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.ProductAPIBase + "/api/productapi/getproduct/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.PUT,
                Url = StaticData.ProductAPIBase + "/api/productapi/updateproduct/" + productDto.ProductId,
                Data = productDto
            });
        }
    }
}
