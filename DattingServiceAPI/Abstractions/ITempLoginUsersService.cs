using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface ITempLoginUsersService
    {
        Task<Guid> AddAsync(LoginUsers user, CancellationToken token);
        Task<int> DeleteAsync(string email, CancellationToken token);
        Task<LoginUsers?> GetAsync(Guid id, CancellationToken token);
        Task<bool> CheckAsync(string email, CancellationToken token);
    }
}