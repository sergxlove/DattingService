using DattingService.Core.Requests;
using Newtonsoft.Json.Linq;

namespace DattingService.Core.Models
{
    public class Users
    {
        public const int MIN_LENGTH_NAME = 2;
        public const int MAX_LENGTH_NAME = 20;
        public const int MIN_AGE = 18;
        public const int MAX_AGE = 100;
        public const int MAX_LENGTH_DESCRIPTION = 250;

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public int Age { get; private set; }

        public string Target { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public string City { get; private set; } = string.Empty;

        public JArray PhotoURL { get; private set; } = new JArray();

        public bool IsActive { get; private set; }

        public bool IsVerify {  get; private set; }

        public static Result<Users> Create(Guid id, string name, int age, string target,
            string description, string city, JArray photoUrl, bool isActive, bool isVerify)
        {
            if (id == Guid.Empty)
                return Result<Users>.Failure("id is empty");
            if (string.IsNullOrEmpty(name))
                return Result<Users>.Failure("name is empty");
            if (string.IsNullOrEmpty(city))
                return Result<Users>.Failure("city is empty");
            if (name.Length < MIN_LENGTH_NAME || name.Length > MAX_LENGTH_NAME)
                return Result<Users>.Failure($"name need is {MIN_LENGTH_NAME} - " +
                    $"{MAX_LENGTH_NAME} symbols");
            if (age < MIN_AGE || age > MAX_AGE)
                return Result<Users>.Failure($"age need is {MIN_AGE} - {MAX_AGE}");
            if (description.Length > MAX_LENGTH_DESCRIPTION)
                return Result<Users>.Failure($"description need is >250 symbols");
            return Result<Users>.Success(new(id, name, age, target, description, city,
                photoUrl,isActive, isVerify));
        }

        private Users(Guid id, string name, int age, string target,string description,
            string city, JArray photoUrl, bool isActive, bool isVerify)
        {
            Id = id;
            Name = name;
            Age = age;
            Target = target;
            Description = description;
            City = city;
            PhotoURL = photoUrl;
            IsActive = isActive;
            IsVerify = isVerify;
        }

        public static Result<Users> Create(Guid id, string name, int age, string target, string description, 
            string city, bool isActive, bool isVerify)
        {
            return Create(id, name, age, target, description, city, new JArray() ,isActive, isVerify);
        }

        public static Result<Users> Create(UsersRequest request)
        {
            return Create(request.Id, request.Name, request.Age, request.Target, 
                request.Description, request.City, request.PhotoURL, request.IsActive,
                request.IsVerify);
        }

        public bool AddUrlPhoto(string url)
        {
            if(PhotoURL?.Count > 1) return false;
            PhotoURL?.Add(url);
            return true;
        }

        public bool RemoveUrlPhoto(string url)
        {
            bool? result = PhotoURL?.Remove(url);
            if (result == false) return false;
            return true;
        }


    }
}
