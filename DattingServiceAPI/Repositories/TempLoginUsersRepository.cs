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

        public async Task<LoginUsers?> GetAsync(Guid id, CancellationToken token)
        {
            var result = await _context.LoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, token);
            if (result == null) return null;
            var user = LoginUsers.Create(result.Id, result.Email, result.Password);
            if (user.IsSuccess) return user.Value;
            return null;
        }

    }
}
