using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using ProfilesServiceAPI.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProfilesServiceAPI.Repositories
{
    public class LoginUsersRepository : ILoginUsersRepository
    {
        private readonly ProfilesDbContext _context;

        public LoginUsersRepository(ProfilesDbContext context)
        {
            _context = context;
        }

        public async Task<Guid?> VerifyAsync(string email, string password, CancellationToken token)
        {
            if (string.IsNullOrEmpty(email)) return null;
            if (string.IsNullOrEmpty(password)) return null;
            var result = await _context.LoginUsers.FirstOrDefaultAsync(a => a.Email == email, token);
            if (result == null) return null;

            string passwordHash = string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(bytes);
            passwordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            if (passwordHash != result.Password) return null;

            return result.Id;
        }

        public async Task<Guid> AddAsync(LoginUsers user, CancellationToken token)
        {
            LoginUsersEntity userEntity = new LoginUsersEntity()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
            };
            await _context.LoginUsers.AddAsync(userEntity, token);
            await _context.SaveChangesAsync(token);
            return userEntity.Id;
        }

        public async Task<int> DeleteAsync(string email, CancellationToken token)
        {
            return await _context.LoginUsers
                .AsNoTracking()
                .Where(a => a.Email == email)
                .ExecuteDeleteAsync(token);
        }

        public async Task<bool> CheckAsync(string email, CancellationToken token)
        {
            var result = await _context.LoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email == email, token);
            if (result is null) return false;
            return true;
        }

        public async Task<int> UpdatePasswordAsync(LoginUsers user, CancellationToken token)
        {
            return await _context.LoginUsers
                .AsNoTracking()
                .Where(a => a.Email == user.Email)
                .ExecuteUpdateAsync(a =>
                a.SetProperty(a => a.Password, user.Password), token);
        }

    }
}
