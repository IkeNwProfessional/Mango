using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Repository.IRepository
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<ApplicationUser?> GetUserByUsernameAsync(string userName);
        Task<ApplicationUser?> GetByIdAsync(string id);
    }
}
