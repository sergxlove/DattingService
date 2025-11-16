using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface IRegistrUserService
    {
        Task<bool> RegistrationAsync(Users user, TokensUser tokenUser, CancellationToken token);
        Task<bool> DeleteUserAsync(Guid id, CancellationToken token);
    }
}