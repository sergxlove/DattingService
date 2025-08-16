using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface ITempLoginUsersRepository
    {
        Task<Guid> AddAsync(LoginUsers user, CancellationToken token);
        Task<int> DeleteAsync(string email, CancellationToken token);
        Task<LoginUsers?> GetAsync(Guid id, CancellationToken token);
    }
}