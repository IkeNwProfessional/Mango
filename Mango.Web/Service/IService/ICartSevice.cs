using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);

        /// <summary>
        /// Kicks off the process to email the cart information to the user by creating messages in the message queue
        /// </summary>
        /// <param name="cartDto"></param>
        /// <returns></returns>
        Task<ResponseDto?> EmailCartAsync(CartDto cartDto);
    }
}
