using DataAccess.Profiles.Postgres;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ProfilesServiceAPI.Repositories
{
    public class UsersLoginRepository
    {
        public UsersLoginRepository(ProfilesDbContext dbContext)
        {
            _context = dbContext;
        }

        private readonly ProfilesDbContext _context;

        public async Task<bool> VerifyAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) return false;
            if (string.IsNullOrEmpty(password)) return false;
            var result = await _context.UsersLogin.FirstOrDefaultAsync(a => a.Username == username);
            if (result == null) return false;

            string passwordHash = string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(bytes);
            passwordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            if (passwordHash != password) return false;

            return true;
        }

        public async Task<Guid> AddAsync(UsersDataForLogin user)
        {

        }
    }
}
