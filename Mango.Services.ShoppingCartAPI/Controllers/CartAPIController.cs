using AutoMapper;
using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper mapper;
        private readonly IProductService productService;
        private readonly IMessageBus messageBus;
        private readonly IConfiguration configuration;
        private readonly ICouponService couponService;
        private readonly ILogger<CartAPIController> _logger;
        private readonly ResponseDto _response;

        public CartAPIController(AppDbContext db, IMapper mapper, IProductService productService, ICouponService couponService, IMessageBus messageBus, IConfiguration configuration, ILogger<CartAPIController> logger)
        {
            this._db = db;
            this.mapper = mapper;
            this.productService = productService;
            this.messageBus = messageBus;
            this.configuration = configuration;
            this._response = new ResponseDto();
            this.couponService = couponService;
            this._logger = logger;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving cart for user: {UserId}", userId);
                CartDto cart = new CartDto
                {
                    CartHeader = mapper.Map<CartHeaderDto>(_db.CartHeaders.First(u => u.UserId == userId))
                };
                cart.CartDetails = mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> productDtos = await productService.GetProducts();
                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                // apply coupon if we have any
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    _logger.LogInformation("Applying coupon {CouponCode} to cart for user: {UserId}", cart.CartHeader.CouponCode, userId);
                    CouponDto coupon = await couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                        _logger.LogInformation("Coupon {CouponCode} applied successfully. Discount: {DiscountAmount}", cart.CartHeader.CouponCode, coupon.DiscountAmount);
                    }
                }

                _response.Result = cart;
                _logger.LogInformation("Successfully retrieved cart for user: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching cart for user: {UserId}", userId);
                _response.IsSuccess = false;
                _response.Message = "Error while fetching the cart.";
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                _logger.LogInformation("Applying coupon {CouponCode} for user: {UserId}", cartDto.CartHeader.CouponCode, cartDto.CartHeader.UserId);
                var cartFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
                _logger.LogInformation("Coupon applied successfully for user: {UserId}", cartDto.CartHeader.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying coupon for user: {UserId}", cartDto.CartHeader.UserId);
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }

            return _response;
        }

        [HttpPost("EmailCartRequest")]
        public async Task<object> EmailCartReqeust([FromBody] CartDto cartDto)
        {
            try
            {
                _logger.LogInformation("Publishing email cart request for user: {UserId}", cartDto.CartHeader.UserId);
                await messageBus.PublishMessage(cartDto, configuration.GetValue<string>("TopicsAndQueueNames:EmailShoppingCart"));
                _response.Result = true;
                _logger.LogInformation("Email cart request published successfully for user: {UserId}", cartDto.CartHeader.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing email cart request for user: {UserId}", cartDto.CartHeader.UserId);
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }

            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert([FromBody] CartDto cartDto)
        {
            try
            {
                _logger.LogInformation("Upserting cart for user: {UserId}", cartDto.CartHeader.UserId);
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    _logger.LogInformation("Creating new cart header for user: {UserId}", cartDto.CartHeader.UserId);
                    // create header and details
                    CartHeader cartHeader = mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                    _logger.LogInformation("Cart header created successfully for user: {UserId}", cartDto.CartHeader.UserId);
                }
                else
                {
                    // if header is not null
                    // check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.First().ProductId
                    && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        _logger.LogInformation("Adding new product to cart for user: {UserId}", cartDto.CartHeader.UserId);
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogInformation("Updating existing product count in cart for user: {UserId}", cartDto.CartHeader.UserId);
                        // update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }

                }
                _response.Result = cartDto;
                _logger.LogInformation("Cart upserted successfully for user: {UserId}", cartDto.CartHeader.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error upserting cart for user: {UserId}", cartDto.CartHeader.UserId);
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                _logger.LogInformation("Removing cart item with ID: {CartDetailsId}", cartDetailsId);
                CartDetails cartDetails = _db.CartDetails.First(u => u.CartDetailsId == cartDetailsId);
                int totalCountOfCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);

                // If there is only 1 item in cart then remove the cart header as well
                if (totalCountOfCartItem == 1)
                {
                    _logger.LogInformation("Removing cart header as it was the last item. CartDetailsId: {CartDetailsId}", cartDetailsId);
                    var cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _db.SaveChangesAsync();
                _response.Result = true;
                _logger.LogInformation("Successfully removed cart item with ID: {CartDetailsId}", cartDetailsId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item with ID: {CartDetailsId}", cartDetailsId);
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
