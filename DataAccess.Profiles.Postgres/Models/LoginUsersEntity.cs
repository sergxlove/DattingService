namespace DataAccess.Profiles.Postgres.Models
{
    public class LoginUsersEntity
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
