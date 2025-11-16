using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface ILoginUsersService
    {
        Task<Guid> AddAsync(LoginUsers user, CancellationToken token);
        Task<bool> CheckAsync(string email, CancellationToken token);
        Task<bool> CheckAsync(Guid id, CancellationToken token);
        Task<int> DeleteAsync(string email, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<int> UpdatePasswordAsync(LoginUsers user, CancellationToken token);
        Task<Guid> VerifyAsync(string email, string password, CancellationToken token);
        Task<string> GetEmailAsync(Guid id, CancellationToken token);
    }
}