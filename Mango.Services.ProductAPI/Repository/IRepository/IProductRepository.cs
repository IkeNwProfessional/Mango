using Mango.Services.ProductAPI.Models;

namespace Mango.Services.ProductAPI.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<Product?> GetProductById(int id);
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}
