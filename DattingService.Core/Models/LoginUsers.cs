using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DattingService.Core.Models
{
    public class LoginUsers
    {
        public const int MIN_LENGTH_STRING = 8;
        public const int MAX_LENGTH_EMAIL = 32;
        public const int MAX_LENGTH_PASSWORD = 64;
        public Guid Id { get; }

        public string Email { get; private set; } = string.Empty;

        public string Password { get; private set; } = string.Empty;

        public static Result<LoginUsers> Create(Guid id, string email, string password)
        {
            if (id == Guid.Empty)
                return Result<LoginUsers>.Failure("id is empty");
            if (string.IsNullOrEmpty(email))
                return Result<LoginUsers>.Failure("email is empty");
            if (string.IsNullOrEmpty(password))
                return Result<LoginUsers>.Failure("password is empty");
            if (email.Length < MIN_LENGTH_STRING || email.Length > MAX_LENGTH_EMAIL)
                return Result<LoginUsers>.Failure($"email need is {MIN_LENGTH_STRING} - " +
                    $"{MAX_LENGTH_EMAIL} symbols");
            if (password.Length < MIN_LENGTH_STRING || password.Length > MAX_LENGTH_PASSWORD)
                return Result<LoginUsers>.Failure($"password need is {MIN_LENGTH_STRING} - " +
                    $"{MAX_LENGTH_PASSWORD} symbols");
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result<LoginUsers>.Failure("Invalid email format");

            return Result<LoginUsers>.Success(new LoginUsers(id, email, password));
        }

        public static Result<LoginUsers> Create(string email, string password)
        {
            return Create(Guid.NewGuid(), email, password);
        }

        private LoginUsers(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = HashPassword(password);
        }

        public Result<LoginUsers> UpadatePassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
                return Result<LoginUsers>.Failure("password is empty");
            if (newPassword.Length < MIN_LENGTH_STRING || newPassword.Length > MAX_LENGTH_PASSWORD)
                return Result<LoginUsers>.Failure($"password need is {MIN_LENGTH_STRING} - " +
                    $"{MAX_LENGTH_PASSWORD} symbols");
            Password = HashPassword(newPassword);
            return Result<LoginUsers>.Success(this);
        }

        private string HashPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
