using DattingService.Core.Models;

namespace DataAccess.Profiles.Postgres.Abstractions
{
    public interface ITokensUserRepository
    {
        Task<Guid> AddAsync(TokensUser tokensUser, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<TokensUser?> GetAsync(Guid id, CancellationToken token);
        Task<int> UpdateAsync(TokensUser tokenUser, CancellationToken token);
    }
}