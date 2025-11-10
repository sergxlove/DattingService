using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Abstractions;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Repositories
{
    public class LoginUsersRepository : ILoginUsersRepository
    {
        private readonly ProfilesDbContext _context;
        private readonly IPasswordHasherService _passwordHasher;

        public LoginUsersRepository(ProfilesDbContext context, IPasswordHasherService passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid?> VerifyAsync(string email, string password, CancellationToken token)
        {
            if (string.IsNullOrEmpty(email)) return null;
            if (string.IsNullOrEmpty(password)) return null;
            LoginUsersEntity? result = await _context.LoginUsers.FirstOrDefaultAsync(a => a.Email == email, token);
            if (result == null) return null;
            if (_passwordHasher.Verify(password, result.Password)) return result.Id;
            return null;
        }

        public async Task<Guid> AddAsync(LoginUsers user, CancellationToken token)
        {
            LoginUsersEntity userEntity = new()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password
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

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _context.LoginUsers
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(token);
        }

        public async Task<bool> CheckAsync(string email, CancellationToken token)
        {
            LoginUsersEntity? result = await _context.LoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email == email, token);
            if (result is null) return false;
            return true;
        }

        public async Task<bool> CheckAsync(Guid id, CancellationToken token)
        {
            LoginUsersEntity? result = await _context.LoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, token);
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

        public async Task<string> GetEmailAsync(Guid id,  CancellationToken token)
        {
            LoginUsersEntity? result = await _context.LoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id  == id, token);
            if (result is null) return string.Empty;
            return result.Email;
        }
    }
}
