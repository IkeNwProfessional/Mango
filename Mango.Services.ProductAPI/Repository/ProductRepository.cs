using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Product> AddProduct(Product product)
        {
            if (product == null)
            {
                _logger.LogError("AddProduct called with null product");
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            try
            {
                _logger.LogInformation("Adding new product: {ProductName}", product.Name);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added product with ID: {ProductId}", product.ProductId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while saving the product: {ProductName}", product.Name);
                throw;
            }
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            _logger.LogInformation("Attempting to delete product with ID: {ProductId}", id);
            _context.Products.Remove(new Product() { ProductId = id });
            var result = await _context.SaveChangesAsync() > 0;
            
            if (result)
            {
                _logger.LogInformation("Successfully deleted product with ID: {ProductId}", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete product with ID: {ProductId}", id);
            }
            
            return result;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            _logger.LogInformation("Retrieving all products");
            var products = await _context.Products.ToListAsync();
            _logger.LogInformation("Retrieved {Count} products", products.Count);
            return products;
        }

        public async Task<Product?> GetProductById(int id)
        {
            _logger.LogInformation("Retrieving product with ID: {ProductId}", id);
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found", id);
            }
            
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", product.ProductId);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated product with ID: {ProductId}", product.ProductId);
            return product;
        }
    }
}
