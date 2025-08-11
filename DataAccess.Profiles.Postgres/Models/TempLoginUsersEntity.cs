namespace DataAccess.Profiles.Postgres.Models
{
    public class TempLoginUsersEntity
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
