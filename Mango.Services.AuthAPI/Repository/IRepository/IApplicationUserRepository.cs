using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Repository.IRepository
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByUsernameAsync(string userName);
        Task<ApplicationUser?> GetByIdAsync(string id);
    }
}
