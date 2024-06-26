using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementWebApp.Models;

namespace UserManagementWebApp.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<bool> UserNameExistsAsync(string username); // Add this method
    }
    public class UserRepository : IUserRepository
    {
        private readonly IAppDbContextFactory _appDbContextFactory;

        public UserRepository(IAppDbContextFactory appDbContextFactory)
        {
            _appDbContextFactory = appDbContextFactory;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using (var db = _appDbContextFactory.CreateDbContext())
            {
                return await db.Users.ToListAsync();
            }
        }

        public async Task<User> GetUserAsync(int id)
        {
            using (var db = _appDbContextFactory.CreateDbContext())
            {
                return await db.Users.FindAsync(id);
            }
        }

        public async Task<User> AddUserAsync(User user)
        {
            using (var db = _appDbContextFactory.CreateDbContext())
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return user;
            }
        }

        public async Task<bool> UserNameExistsAsync(string username)
        {
            using (var db = _appDbContextFactory.CreateDbContext())
            {
                return await db.Users.AnyAsync(u => u.Username == username);
            }
        }
    }
}
