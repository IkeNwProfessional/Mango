using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Mango.Services.CouponAPI.Repository.IRepository;
using Mango.Services.CouponAPI.Services.IService;

namespace Mango.Services.CouponAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository couponRepository;
        private readonly IMapper mapper;

        public CouponService(ICouponRepository couponRepository, IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.mapper = mapper;
        }

        public async Task<CouponDto?> CreateCoupon(CouponDto coupon)
        {
            if (coupon == null)
            {
                return new CouponDto();
            }

            var couponEntity = mapper.Map<Coupon>(coupon);
            var addedCoupon = await couponRepository.AddCoupon(couponEntity);
            return mapper.Map<CouponDto>(addedCoupon);
        }

        public async Task<bool> DeleteCoupon(int id)
        {
            return await couponRepository.DeleteCoupon(id);
        }

        public async Task<IEnumerable<CouponDto>> GetAllCoupons()
        {
            var coupons = await couponRepository.GetAllCoupons();
            return mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<CouponDto?> GetCouponById(int id)
        {
            var coupon = await couponRepository.GetCouponById(id);
            return mapper.Map<CouponDto>(coupon);
        }

        public async Task<CouponDto?> GetCouponByCode(string code)
        {
            var coupon = await couponRepository.GetCouponByCode(code);
            return mapper.Map<CouponDto>(coupon);
        }

        public async Task<CouponDto?> UpdateCoupon(int id, CouponDto coupon)
        {
            var existingCoupon = await couponRepository.GetCouponById(id);
            if (existingCoupon == null)
            {
                return null;
            }

            existingCoupon.CouponCode = coupon.CouponCode;
            existingCoupon.DiscountAmount = coupon.DiscountAmount;
            existingCoupon.MinAmount = coupon.MinAmount;

            var updatedCoupon = await couponRepository.UpdateCoupon(existingCoupon);
            return mapper.Map<CouponDto>(updatedCoupon);
        }
    }
}
