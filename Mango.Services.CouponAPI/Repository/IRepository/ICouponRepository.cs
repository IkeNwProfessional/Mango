using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetCouponById(int id);
        Task<Coupon?> GetCouponByCode(string code);
        Task<Coupon> AddCoupon(Coupon coupon);
        Task<Coupon> UpdateCoupon(Coupon coupon);
        Task<bool> DeleteCoupon(int id);
        Task<IEnumerable<Coupon>> GetAllCoupons();
    }
}
