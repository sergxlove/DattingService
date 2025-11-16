namespace DataAccess.Profiles.Postgres.Models
{
    public class TokensUserEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Ended { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
