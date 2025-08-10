using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Repositories
{
    public class InterestsRepository : IInterestsRepository
    {
        private readonly ProfilesDbContext _context;

        public InterestsRepository(ProfilesDbContext context)
        {
            _context = context;
        }

        public async Task<JArray> GetAsync(Guid id)
        {
            var result = await _context.Interests
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (result is null) return new JArray();
            return result.SelectInterests;
        }

        public async Task<Guid> AddAsync(Interests interest)
        {
            InterestsEntity interestsEntity = new()
            {
                Id = interest.Id,
                SelectInterests = interest.SelectInterests,
            };
            await _context.Interests.AddAsync(interestsEntity);
            await _context.SaveChangesAsync();
            return interestsEntity.Id;
        }

        public async Task<int> UpdateAsync(Interests interest)
        {
            return await _context.Interests
                .AsNoTracking()
                .Where(a => a.Id == interest.Id)
                .ExecuteUpdateAsync(a =>
                a.SetProperty(a => a.SelectInterests, interest.SelectInterests));
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _context.Interests
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> CheckAsync(Guid id)
        {
            var result = await _context.Interests
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (result is null) return false;
            return true;
        }
    }
}
