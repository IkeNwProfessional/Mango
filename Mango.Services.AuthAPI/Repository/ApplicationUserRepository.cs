using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Repository.IRepository;

namespace Mango.Services.AuthAPI.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        public Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetByUsernameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
