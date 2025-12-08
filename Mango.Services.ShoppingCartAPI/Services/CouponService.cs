using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CouponService> _logger;

        public CouponService(IHttpClientFactory clientFactory, ILogger<CouponService> logger)
        {
            _httpClientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            try
            {
                _logger.LogInformation("Fetching coupon with code: {CouponCode} from Coupon API", couponCode);
                var client = _httpClientFactory.CreateClient("Coupon");
                var response = await client.GetAsync($"api/coupon/GetByCode/{couponCode}");
                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

                if (resp.IsSuccess)
                {
                    var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
                    _logger.LogInformation("Successfully fetched coupon: {CouponCode} with discount: {DiscountAmount}", couponCode, coupon?.DiscountAmount ?? 0);
                    return coupon;
                }
                else
                {
                    _logger.LogWarning("Coupon API returned unsuccessful response for code: {CouponCode}. Message: {Message}", couponCode, resp.Message);
                    return new CouponDto();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching coupon with code: {CouponCode} from Coupon API", couponCode);
                return new CouponDto();
            }
        }
    }
}
