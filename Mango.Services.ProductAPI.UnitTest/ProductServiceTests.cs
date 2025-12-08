using AutoMapper;
using FluentAssertions;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repository.IRepository;
using Mango.Services.ProductAPI.Services;
using NSubstitute;

namespace Mango.Services.ProductAPI.UnitTest
{
    [TestClass]
    public class ProductServiceTests
    {
        private IProductRepository _productRepository;
        private IMapper _mapper;
        private ProductService _productService;

        [TestInitialize]
        public void Setup()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _productService = new ProductService(_productRepository, _mapper);
        }

        #region CreateProduct Tests

        [TestMethod]
        public async Task CreateProduct_WithValidProduct_ShouldReturnMappedProductDto()
        {
            // Arrange
            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
                CategoryName = "Test Category",
                ImageUrl = "http://test.com/image.jpg"
            };

            var productEntity = new Product
            {
                ProductId = 1,
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
                CategoryName = "Test Category",
                ImageUrl = "http://test.com/image.jpg"
            };

            _mapper.Map<Product>(productDto).Returns(productEntity);
            _productRepository.AddProduct(productEntity).Returns(productEntity);
            _mapper.Map<ProductDto>(productEntity).Returns(productDto);

            // Act
            var result = await _productService.CreateProduct(productDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(productDto);
            await _productRepository.Received(1).AddProduct(Arg.Any<Product>());
            _mapper.Received(1).Map<Product>(productDto);
            _mapper.Received(1).Map<ProductDto>(productEntity);
        }

        [TestMethod]
        public async Task CreateProduct_WithNullProduct_ShouldReturnEmptyProductDto()
        {
            // Act
            var result = await _productService.CreateProduct(null);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProductDto>();
            await _productRepository.DidNotReceive().AddProduct(Arg.Any<Product>());
        }

        #endregion

        #region GetProductById Tests

        [TestMethod]
        public async Task GetProductById_WithExistingId_ShouldReturnMappedProductDto()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                ProductId = productId,
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
                CategoryName = "Test Category",
                ImageUrl = "http://test.com/image.jpg"
            };

            var productDto = new ProductDto
            {
                ProductId = productId,
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
                CategoryName = "Test Category",
                ImageUrl = "http://test.com/image.jpg"
            };

            _productRepository.GetProductById(productId).Returns(product);
            _mapper.Map<ProductDto>(product).Returns(productDto);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(productDto);
            await _productRepository.Received(1).GetProductById(productId);
            _mapper.Received(1).Map<ProductDto>(product);
        }

        [TestMethod]
        public async Task GetProductById_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            var productId = 999;
            _productRepository.GetProductById(productId).Returns((Product?)null);
            _mapper.Map<ProductDto>((Product?)null).Returns((ProductDto?)null);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            result.Should().BeNull();
            await _productRepository.Received(1).GetProductById(productId);
        }

        #endregion

        #region GetAllProducts Tests

        [TestMethod]
        public async Task GetAllProducts_WithProducts_ShouldReturnMappedProductDtos()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Product 1", Price = 100, Description = "Desc 1", CategoryName = "Cat 1", ImageUrl = "url1" },
                new Product { ProductId = 2, Name = "Product 2", Price = 200, Description = "Desc 2", CategoryName = "Cat 2", ImageUrl = "url2" }
            };

            var productDtos = new List<ProductDto>
            {
                new ProductDto { ProductId = 1, Name = "Product 1", Price = 100, Description = "Desc 1", CategoryName = "Cat 1", ImageUrl = "url1" },
                new ProductDto { ProductId = 2, Name = "Product 2", Price = 200, Description = "Desc 2", CategoryName = "Cat 2", ImageUrl = "url2" }
            };

            _productRepository.GetAllProducts().Returns(products);
            _mapper.Map<IEnumerable<ProductDto>>(products).Returns(productDtos);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(productDtos);
            await _productRepository.Received(1).GetAllProducts();
            _mapper.Received(1).Map<IEnumerable<ProductDto>>(products);
        }

        [TestMethod]
        public async Task GetAllProducts_WithNoProducts_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyProducts = new List<Product>();
            var emptyProductDtos = new List<ProductDto>();

            _productRepository.GetAllProducts().Returns(emptyProducts);
            _mapper.Map<IEnumerable<ProductDto>>(emptyProducts).Returns(emptyProductDtos);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            await _productRepository.Received(1).GetAllProducts();
        }

        #endregion

        #region UpdateProduct Tests

        [TestMethod]
        public async Task UpdateProduct_WithExistingProduct_ShouldUpdateAndReturnProductDto()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new Product
            {
                ProductId = productId,
                Name = "Old Name",
                Price = 100,
                Description = "Old Description",
                CategoryName = "Old Category",
                ImageUrl = "old-url"
            };

            var updateDto = new ProductDto
            {
                ProductId = productId,
                Name = "New Name",
                Price = 150,
                Description = "New Description",
                CategoryName = "New Category",
                ImageUrl = "new-url"
            };

            var updatedProduct = new Product
            {
                ProductId = productId,
                Name = "New Name",
                Price = 150,
                Description = "New Description",
                CategoryName = "New Category",
                ImageUrl = "new-url"
            };

            _productRepository.GetProductById(productId).Returns(existingProduct);
            _productRepository.UpdateProduct(existingProduct).Returns(updatedProduct);
            _mapper.Map<ProductDto>(updatedProduct).Returns(updateDto);

            // Act
            var result = await _productService.UpdateProduct(productId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updateDto);
            existingProduct.Name.Should().Be("New Name");
            existingProduct.Price.Should().Be(150);
            existingProduct.Description.Should().Be("New Description");
            existingProduct.CategoryName.Should().Be("New Category");
            existingProduct.ImageUrl.Should().Be("new-url");
            await _productRepository.Received(1).GetProductById(productId);
            await _productRepository.Received(1).UpdateProduct(existingProduct);
            _mapper.Received(1).Map<ProductDto>(updatedProduct);
        }

        [TestMethod]
        public async Task UpdateProduct_WithNonExistingProduct_ShouldReturnNull()
        {
            // Arrange
            var productId = 999;
            var updateDto = new ProductDto
            {
                ProductId = productId,
                Name = "New Name",
                Price = 150,
                Description = "New Description",
                CategoryName = "New Category",
                ImageUrl = "new-url"
            };

            _productRepository.GetProductById(productId).Returns((Product?)null);

            // Act
            var result = await _productService.UpdateProduct(productId, updateDto);

            // Assert
            result.Should().BeNull();
            await _productRepository.Received(1).GetProductById(productId);
            await _productRepository.DidNotReceive().UpdateProduct(Arg.Any<Product>());
            _mapper.DidNotReceive().Map<ProductDto>(Arg.Any<Product>());
        }

        #endregion

        #region DeleteProduct Tests

        [TestMethod]
        public async Task DeleteProduct_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var productId = 5;
            _productRepository.DeleteProduct(productId).Returns(true);

            // Act
            var result = await _productService.DeleteProduct(productId);

            // Assert
            result.Should().BeTrue();
            await _productRepository.Received(1).DeleteProduct(productId);
        }

        [TestMethod]
        public async Task DeleteProduct_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var productId = 999;
            _productRepository.DeleteProduct(productId).Returns(false);

            // Act
            var result = await _productService.DeleteProduct(productId);

            // Assert
            result.Should().BeFalse();
            await _productRepository.Received(1).DeleteProduct(productId);
        }

        #endregion
    }
}
