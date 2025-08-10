using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface IUsersRepository
    {
        Task<Guid> AddAsync(Users user);
        Task<int> DeleteAsync(Guid id);
        Task<Users?> GetByIdAsync(Guid id);
        Task<int> ActiveAsync(Guid id);
        Task<int> VerifyAsync(Guid id);
        Task<int> InactiveAsync(Guid id);
    }
}
