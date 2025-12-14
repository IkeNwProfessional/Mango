namespace Mango.Services.EmailAPI.Models.Dto
{
    public class CartDto
    {
        // Contains information about cart total, userId, couponCode, discountAmount
        public CartHeaderDto CartHeader { get; set; }
        // List of products for a user in the cart with details like productId, count
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }   
    }
}
