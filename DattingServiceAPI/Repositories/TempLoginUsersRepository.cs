using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Repositories
{
    public class TempLoginUsersRepository : ITempLoginUsersRepository
    {
        private readonly ProfilesDbContext _context;
        public TempLoginUsersRepository(ProfilesDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(LoginUsers user, CancellationToken token)
        {
            TempLoginUsersEntity userEntity = new()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
            };
            await _context.TempLoginUsers.AddAsync(userEntity, token);
            await _context.SaveChangesAsync(token);
            return userEntity.Id;
        }

        public async Task<int> DeleteAsync(string email, CancellationToken token)
        {
            return await _context.TempLoginUsers
                .AsNoTracking()
                .Where(a => a.Email == email)
                .ExecuteDeleteAsync(token);
        }

        public async Task<LoginUsers?> GetAsync(Guid id, CancellationToken token)
        {
            TempLoginUsersEntity? result = await _context.TempLoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, token);
            if (result == null) return null;
            Result<LoginUsers> user = LoginUsers.Create(result.Id, result.Email, 
                result.Password, false);
            if (user.IsSuccess) return user.Value;
            return null;
        }

        public async Task<bool> CheckAsync(string email, CancellationToken token)
        {
            TempLoginUsersEntity? result = await _context.TempLoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email == email, token);
            if (result == null) return false;
            return true;
        }
    }
}
