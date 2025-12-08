using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.Services.IService
{
    public interface ICouponService
    {
        Task<CouponDto?> GetCouponById(int id);
        Task<CouponDto?> GetCouponByCode(string code);
        Task<CouponDto?> CreateCoupon(CouponDto coupon);
        Task<CouponDto?> UpdateCoupon(int id, CouponDto coupon);
        Task<bool> DeleteCoupon(int id);
        Task<IEnumerable<CouponDto>> GetAllCoupons();
    }
}
