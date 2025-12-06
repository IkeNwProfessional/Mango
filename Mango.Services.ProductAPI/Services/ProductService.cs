using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repository.IRepository;
using Mango.Services.ProductAPI.Services.IService;

namespace Mango.Services.ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        public async Task<ProductDto?> CreateProduct(ProductDto product)
        {
            if (product == null)
            {
                return new ProductDto();
            }

            var productEntity = mapper.Map<Product>(product);
            var addedProduct = await productRepository.AddProduct(productEntity);
            return mapper.Map<ProductDto>(addedProduct);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await productRepository.DeleteProduct(id);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await productRepository.GetAllProducts();
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var product = await productRepository.GetProductById(id);
            return mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto?> UpdateProduct(int id, ProductDto product)
        {
            var existingProduct = await productRepository.GetProductById(id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.CategoryName = product.CategoryName;
            existingProduct.ImageUrl = product.ImageUrl;

            var updatedProduct = await productRepository.UpdateProduct(existingProduct);
            return mapper.Map<ProductDto>(updatedProduct);
        }
    }
}
