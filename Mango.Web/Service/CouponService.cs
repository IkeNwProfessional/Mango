using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;

        public CouponService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Data = couponDto,
                Url = StaticData.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> DeleteCouponsAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.DELETE,
                Url = StaticData.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = StaticData.ApiType.PUT,
                Url = StaticData.CouponAPIBase + "/api/coupon"
            });
        }
    }
}
