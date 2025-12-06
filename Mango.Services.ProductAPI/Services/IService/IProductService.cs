using Mango.Services.ProductAPI.Models.Dto;
namespace Mango.Services.ProductAPI.Services.IService
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductById(int id);
        Task<ProductDto?> CreateProduct(ProductDto product);
        Task<ProductDto?> UpdateProduct(int id, ProductDto product);
        Task<bool> DeleteProduct(int id);
        Task<IEnumerable<ProductDto>> GetAllProducts();
    }
}
