using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly AppDbContext _db;

        public ApplicationUserRepository(AppDbContext db)
        {
            this._db = db;
        }
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<ApplicationUser?> GetUserByUsernameAsync(string userName)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _db.ApplicationUsers.FindAsync(id);
        }
    }
}
