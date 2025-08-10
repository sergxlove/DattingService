using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface ILoginUsersRepository
    {
        Task<Guid> AddAsync(LoginUsers user);
        Task<bool> CheckAsync(string email);
        Task<int> DeleteAsync(string email);
        Task<int> UpdatePasswordAsync(LoginUsers user);
        Task<Guid?> VerifyAsync(string email, string password);
    }
}