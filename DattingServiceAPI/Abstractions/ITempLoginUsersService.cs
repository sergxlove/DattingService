using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface ITempLoginUsersService
    {
        Task<Guid> AddAsync(LoginUsers user);
        Task<int> DeleteAsync(string email);
        Task<LoginUsers?> GetAsync(Guid id);
    }
}