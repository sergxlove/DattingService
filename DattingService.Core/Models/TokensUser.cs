namespace DattingService.Core.Models
{
    public class TokensUser
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public DateTime Created { get; }
        public DateTime Ended { get; }
        public string Role { get; } = string.Empty;

        public static Result<TokensUser> Create(Guid id,  Guid userId, DateTime created, 
            DateTime ended, string role)
        {
            if (id == Guid.Empty)
                return Result<TokensUser>.Failure("id is null");
            if (userId == Guid.Empty)
                return Result<TokensUser>.Failure("id user is null");            
            return Result<TokensUser>.Success(new TokensUser(id, userId, created, ended, role));
        }

        private TokensUser(Guid id, Guid userId, DateTime created,
            DateTime ended, string role)
        {
            Id = id;
            UserId = userId;
            Created = created;
            Ended = ended;
            Role = role;
        }
    }
}
