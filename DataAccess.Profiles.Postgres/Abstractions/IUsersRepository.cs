using DattingService.Core.Models;

namespace DataAccess.Profiles.Postgres.Abstractions
{
    public interface IUsersRepository
    {
        Task<int> ActiveAsync(Guid id, CancellationToken token);
        Task<Guid> AddAsync(Users user, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<Users?> GetByIdAsync(Guid id, CancellationToken token);
        Task<int> InactiveAsync(Guid id, CancellationToken token);
        Task<int> UpdateAsync(Users user, CancellationToken token);
        Task<int> VerifyAsync(Guid id, CancellationToken token);
    }
}