using DattingService.Core.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface ITokensUserService
    {
        Task<Guid> AddAsync(TokensUser tokensUser, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<TokensUser?> GetAsync(Guid id, CancellationToken token);
        Task<int> UpdateAsync(TokensUser tokenUser, CancellationToken token);
    }
}