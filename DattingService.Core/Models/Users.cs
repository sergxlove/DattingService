namespace DattingService.Core.Models
{
    public class Users
    {
        public const int MIN_LENGTH_STRING = 1;
        public const int MAX_LENGTH_STRING = 40;
        public const int MAX_LENGTH_DESCRIPTION = 200;
        public Guid Id { get; }

        public string Name { get; } = string.Empty;

        public int Age { get;}

        public string Description { get; } = string.Empty;

        public string City { get; } = string.Empty;

        public string PhotoURL { get; } = string.Empty;

        public bool IsActive { get; } 

        public static (Users? user, string error) Create(string name, int age,
            string description, string city, string photoURL, bool isActive)
        {
            Users? users = null;
            string error = string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                error = "name is null";
                return (users, error);
            }

            if(!string.IsNullOrEmpty(description))
            {
                error = "description is null";
                return (users, error);
            }

            if(!string.IsNullOrEmpty(city))
            {
                error = "city is null";
                return (users, error);
            }

            if(age < 18 || age > 100)
            {
                error = "age invalid";
                return (users, error);
            }

            if (name.Length < MIN_LENGTH_STRING || name.Length > MAX_LENGTH_STRING)
            {
                error = "name invalid";
                return (users, error);
            }

            if(description.Length > MAX_LENGTH_DESCRIPTION)
            {
                error = "description invalid";
                return (users, error);
            }

            if(city.Length < MIN_LENGTH_STRING ||  city.Length > MAX_LENGTH_STRING)
            {
                error = "city is null";
                return (users, error);
            }

            users = new Users(name, age, description, city, photoURL, isActive);
            return (users, error);
        }

        private Users(string name, int age,
            string description, string city, string photoURL, bool isActive)
        {
            Id = Guid.NewGuid();
            Name = name;
            Age = age;
            Description = description;
            City = city;
            PhotoURL = photoURL;
            IsActive = isActive;
        }
    }
}
