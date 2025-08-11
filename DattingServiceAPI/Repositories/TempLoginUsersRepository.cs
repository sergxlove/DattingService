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

        public async Task<Guid> AddAsync(LoginUsers user)
        {
            LoginUsersEntity userEntity = new LoginUsersEntity()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
            };
            await _context.LoginUsers.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }

        public async Task<int> DeleteAsync(string email)
        {
            return await _context.LoginUsers
                .AsNoTracking()
                .Where(a => a.Email == email)
                .ExecuteDeleteAsync();
        }

        public async Task<LoginUsers?> GetAsync(Guid id)
        {
            var result = await _context.LoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (result == null) return null;
            var user = LoginUsers.Create(result.Id, result.Email, result.Password);
            if (user.IsSuccess) return user.Value;
            return null;
        }

    }
}
