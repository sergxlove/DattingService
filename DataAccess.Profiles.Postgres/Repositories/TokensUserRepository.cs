using DataAccess.Profiles.Postgres.Abstractions;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Profiles.Postgres.Repositories
{
    public class TokensUserRepository : ITokensUserRepository
    {
        private readonly ProfilesDbContext _context;

        public TokensUserRepository(ProfilesDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(TokensUser tokensUser, CancellationToken token)
        {
            TokensUserEntity tokensEntity = new()
            {
                Id = tokensUser.Id,
                UserId = tokensUser.UserId,
                Created = tokensUser.Created,
                Ended = tokensUser.Ended,
                Role = tokensUser.Role,
            };
            await _context.TokensUsers.AddAsync(tokensEntity, token);
            await _context.SaveChangesAsync(token);
            return tokensEntity.Id;
        }

        public async Task<TokensUser?> GetAsync(Guid id, CancellationToken token)
        {
            TokensUserEntity? tokens = await _context.TokensUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, token);
            if (tokens is null) return null;
            Result<TokensUser> tokenUser = TokensUser.Create(tokens.Id, tokens.UserId, tokens.Created,
                tokens.Ended, tokens.Role);
            if (string.IsNullOrEmpty(tokenUser.Error)) return null;
            return tokenUser.Value;
        }

        public async Task<int> UpdateAsync(TokensUser tokenUser, CancellationToken token)
        {
            return await _context.TokensUsers
                .AsNoTracking()
                .Where(x => x.UserId == tokenUser.UserId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(s => s.Id, tokenUser.Id)
                .SetProperty(s => s.Created, tokenUser.Created)
                .SetProperty(s => s.Ended, tokenUser.Ended), token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _context.TokensUsers
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
