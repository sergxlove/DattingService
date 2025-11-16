using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Abstractions;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Profiles.Postgres.Repositories
{
    public class InterestsRepository : IInterestsRepository
    {
        private readonly ProfilesDbContext _context;

        public InterestsRepository(ProfilesDbContext context)
        {
            _context = context;
        }

        public async Task<int[]> GetAsync(Guid id, CancellationToken token)
        {
            InterestsEntity? result = await _context.Interests
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, token);
            if (result is null) return Array.Empty<int>();
            return result.SelectInterests;
        }

        public async Task<Guid> AddAsync(Interests interest, CancellationToken token)
        {
            InterestsEntity interestsEntity = new()
            {
                Id = interest.Id,
                SelectInterests = interest.SelectInterests,
            };
            await _context.Interests.AddAsync(interestsEntity, token);
            await _context.SaveChangesAsync(token);
            return interestsEntity.Id;
        }

        public async Task<int> UpdateAsync(Interests interest, CancellationToken token)
        {
            return await _context.Interests
                .AsNoTracking()
                .Where(a => a.Id == interest.Id)
                .ExecuteUpdateAsync(a =>
                a.SetProperty(a => a.SelectInterests, interest.SelectInterests), token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _context.Interests
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(token);
        }

        public async Task<bool> CheckAsync(Guid id, CancellationToken token)
        {
            InterestsEntity? result = await _context.Interests
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, token);
            if (result is null) return false;
            return true;
        }
    }
}
