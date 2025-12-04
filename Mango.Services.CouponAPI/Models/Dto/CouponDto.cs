namespace Mango.Services.CouponAPI.Models.Dto;

// <summary>
/// Dto for transferring coupon data between layers
/// Dtos are important for encapsulating data and ensuring only necessary information is exposed
// </summary>
public class CouponDto
{
    public int CouponId { get; set; }
    public string CouponCode { get; set; }
    public double DiscountAmount { get; set; }
    public int MinAmount { get; set; }
}
