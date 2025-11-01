using System.Text.RegularExpressions;
using DattingService.Core.Abstractions;
using DattingService.Core.Services;

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

        public static Result<LoginUsers> Create(Guid id, string email, string password, 
            IPasswordHasherService passwordHasher)
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

            return Result<LoginUsers>.Success(new LoginUsers(id, email, password, passwordHasher));
        }

        public static Result<LoginUsers> Create(Guid id, string email, string password)
        {
            PasswordHasherService passwordHasher = new PasswordHasherService();
            return Create(id, email, password, passwordHasher);
        }

        public static Result<LoginUsers> Create(string email, string password,
            IPasswordHasherService passwordHasher)
        {
            return Create(Guid.NewGuid(), email, password, passwordHasher);
        }

        public static Result<LoginUsers> Create(string email, string password)
        {
            PasswordHasherService passwordHasher = new PasswordHasherService();
            return Create(email, password, passwordHasher);
        }

        public static Result<LoginUsers> Create(Guid id, string email, string password, 
            bool isNeedHashPassword)
        {
            if(isNeedHashPassword)
            {
                PasswordHasherService passwordHasher = new PasswordHasherService();
                return Create(email, password, passwordHasher);
            }
            return Result<LoginUsers>.Success(new LoginUsers(id, email, password));
        }

        private LoginUsers(Guid id, string email, string password, IPasswordHasherService passwordHasher)
        {
            Id = id;
            Email = email;
            Password = passwordHasher.Hash(password);
        }

        private LoginUsers(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public Result<LoginUsers> UpdatePassword(string newPassword,
            IPasswordHasherService passwordHasher)
        {
            if (string.IsNullOrEmpty(newPassword))
                return Result<LoginUsers>.Failure("password is empty");
            if (newPassword.Length < MIN_LENGTH_STRING || newPassword.Length > MAX_LENGTH_PASSWORD)
                return Result<LoginUsers>.Failure($"password need is {MIN_LENGTH_STRING} - " +
                    $"{MAX_LENGTH_PASSWORD} symbols");
            Password = passwordHasher.Hash(newPassword);
            return Result<LoginUsers>.Success(this);
        }

        public Result<LoginUsers> UpdatePassword(string newPassword)
        {
            PasswordHasherService passwordHasherService = new PasswordHasherService();
            return UpdatePassword(newPassword, passwordHasherService);
        }

        public static bool Verify(string password, string passwordHash,IPasswordHasherService passwordHasher)
        {
            return passwordHasher.Verify(password, passwordHash);
        }

    }
}
