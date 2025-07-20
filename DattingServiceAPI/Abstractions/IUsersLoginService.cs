using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface IUsersLoginService
    {
        Task<Guid> AddAsync(UsersDataForLogin user);
        Task<bool> CheckAsync(string username);
        Task<int> DeleteAsync(string username);
        Task<int> UpdatePasswordAsync(UsersDataForLogin user);
        Task<bool> VerifyAsync(string username, string password);
    }
}
