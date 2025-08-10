using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public UsersRepository(ProfilesDbContext context)
        {
            _context = context;
        }

        private readonly ProfilesDbContext _context;

        public async Task<Users?> GetByIdAsync(Guid id)
        {
            var result = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (result == null) return null;
            var user = Users.Create(result.Id, result.Name, result.Age, result.Target,
                result.Description, result.City, result.PhotoURL, result.IsActive, result.IsVerify);
            return user.Value;
        }

        public async Task<Guid> AddAsync(Users user)
        {
            UsersEntity userEntity = new UsersEntity()
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Target = user.Target,
                Description = user.Description,
                City = user.City,
                PhotoURL = user.PhotoURL,
                IsActive = user.IsActive,
                IsVerify = user.IsVerify
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<int> ActiveAsync(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where (a => a.Id == id)
                .ExecuteUpdateAsync(a => 
                    a.SetProperty(b => b.IsActive, true));
        }

        public async Task<int> VerifyAsync(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(a =>
                    a.SetProperty(b => b.IsVerify, true));
        }

        public async Task<int> InactiveAsync(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(a =>
                    a.SetProperty(b => b.IsActive, false));
        }
    }
}
