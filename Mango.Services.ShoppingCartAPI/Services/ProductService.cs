using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services.IService;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IHttpClientFactory clientFactory, ILogger<ProductService> logger)
        {
            _httpClientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                _logger.LogInformation("Fetching products from Product API");
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync($"api/ProductAPI/GetAllProducts");
                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

                if (resp.IsSuccess)
                {
                    var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
                    _logger.LogInformation("Successfully fetched {Count} products from Product API", products?.Count() ?? 0);
                    return products;
                }
                else
                {
                    _logger.LogWarning("Product API returned unsuccessful response. Message: {Message}", resp.Message);
                    return new List<ProductDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products from Product API");
                return new List<ProductDto>();
            }
        }
    }
}
