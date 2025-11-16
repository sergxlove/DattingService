using DataAccess.Profiles.Postgres.Abstractions;
using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{
    public class TokensUserService : ITokensUserService
    {
        private readonly ITokensUserRepository _repository;
        public TokensUserService(ITokensUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> AddAsync(TokensUser tokensUser, CancellationToken token)
        {
            return await _repository.AddAsync(tokensUser, token);
        }
        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }
        public async Task<TokensUser?> GetAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetAsync(id, token);
        }
        public async Task<int> UpdateAsync(TokensUser tokenUser, CancellationToken token)
        {
            return await _repository.UpdateAsync(tokenUser, token);
        }
    }
}
