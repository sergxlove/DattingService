namespace DataAccess.Profiles.Postgres.Models
{
    public class UsersEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Description { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string PhotoURL { get; set; } = string.Empty;

    }
}
