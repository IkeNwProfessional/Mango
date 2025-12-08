using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CouponRepository> _logger;

        public CouponRepository(AppDbContext context, ILogger<CouponRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Coupon> AddCoupon(Coupon coupon)
        {
            if (coupon == null)
            {
                _logger.LogError("AddCoupon called with null coupon");
                throw new ArgumentNullException(nameof(coupon), "Coupon cannot be null");
            }

            try
            {
                _logger.LogInformation("Adding new coupon with code: {CouponCode}", coupon.CouponCode);
                _context.Coupons.Add(coupon);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added coupon with ID: {CouponId}", coupon.CouponId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while saving the coupon with code: {CouponCode}", coupon.CouponCode);
                throw;
            }
            return coupon;
        }

        public async Task<bool> DeleteCoupon(int id)
        {
            _logger.LogInformation("Attempting to delete coupon with ID: {CouponId}", id);
            _context.Coupons.Remove(new Coupon() { CouponId = id });
            var result = await _context.SaveChangesAsync() > 0;
            
            if (result)
            {
                _logger.LogInformation("Successfully deleted coupon with ID: {CouponId}", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete coupon with ID: {CouponId}", id);
            }
            
            return result;
        }

        public async Task<IEnumerable<Coupon>> GetAllCoupons()
        {
            _logger.LogInformation("Retrieving all coupons");
            var coupons = await _context.Coupons.ToListAsync();
            _logger.LogInformation("Retrieved {Count} coupons", coupons.Count);
            return coupons;
        }

        public async Task<Coupon?> GetCouponById(int id)
        {
            _logger.LogInformation("Retrieving coupon with ID: {CouponId}", id);
            var coupon = await _context.Coupons.FindAsync(id);
            
            if (coupon == null)
            {
                _logger.LogWarning("Coupon with ID: {CouponId} not found", id);
            }
            
            return coupon;
        }

        public async Task<Coupon?> GetCouponByCode(string code)
        {
            _logger.LogInformation("Retrieving coupon with code: {CouponCode}", code);
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.CouponCode.ToLower() == code.ToLower());
            
            if (coupon == null)
            {
                _logger.LogWarning("Coupon with code: {CouponCode} not found", code);
            }
            
            return coupon;
        }

        public async Task<Coupon> UpdateCoupon(Coupon coupon)
        {
            _logger.LogInformation("Updating coupon with ID: {CouponId}", coupon.CouponId);
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated coupon with ID: {CouponId}", coupon.CouponId);
            return coupon;
        }
    }
}
