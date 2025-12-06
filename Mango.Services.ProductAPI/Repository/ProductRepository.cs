using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Product> AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null");

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while saving the product: {e.Message}");
                throw; // Re-throw the exception after logging it
            }
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            _context.Products.Remove(new Product() { ProductId = id });
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
