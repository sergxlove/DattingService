using DattingService.Core.Models;

namespace DataAccess.Profiles.Postgres.Abstractions
{
    public interface IInterestsRepository
    {
        Task<Guid> AddAsync(Interests interest, CancellationToken token);
        Task<bool> CheckAsync(Guid id, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<int[]> GetAsync(Guid id, CancellationToken token);
        Task<int> UpdateAsync(Interests interest, CancellationToken token);
    }
}