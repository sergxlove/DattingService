using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using ProfilesServiceAPI.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProfilesServiceAPI.Repositories
{
    public class UsersLoginRepository : IUsersLoginRepository
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
            UserDataForLoginEntity userEntity = new UserDataForLoginEntity()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
            };
            await _context.UsersLogin.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }

        public async Task<int> DeleteAsync(string username)
        {
            return await _context.UsersLogin
                .AsNoTracking()
                .Where(a => a.Username == username)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> CheckAsync(string username)
        {
            var result = await _context.UsersLogin
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Username == username);
            if (result is null) return false;
            return true;
        }

        public async Task<int> UpdatePasswordAsync(UsersDataForLogin user)
        {
            return await _context.UsersLogin
                .AsNoTracking()
                .Where(a => a.Username == user.Username)
                .ExecuteUpdateAsync(a =>
                a.SetProperty(a => a.Password, user.Password));
        }
    }
}
