using AutoMapper;
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
    }
}
