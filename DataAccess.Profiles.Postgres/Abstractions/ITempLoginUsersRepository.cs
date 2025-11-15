using DattingService.Core.Models;

namespace DataAccess.Profiles.Postgres.Abstractions
{
    public interface ITempLoginUsersRepository
    {
        Task<Guid> AddAsync(LoginUsers user, CancellationToken token);
        Task<bool> CheckAsync(string email, CancellationToken token);
        Task<int> DeleteAsync(string email, CancellationToken token);
        Task<LoginUsers?> GetAsync(Guid id, CancellationToken token);
    }
}