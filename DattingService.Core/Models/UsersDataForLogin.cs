using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Security.Cryptography;

namespace DattingService.Core.Models
{
    public class UsersDataForLogin
    {
        public const int MIN_LENGTH_STRING = 8;
        public const int MAX_LENGTH_STRING = 128;
        public Guid Id { get; }

        public string Username { get; } = string.Empty;

        public string Password { get; } = string.Empty;

        public Users? User { get; }

        private UsersDataForLogin(Guid id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public (UsersDataForLogin? user, string error) Create(string username, string password)
        {
            UsersDataForLogin? user = null;
            string error = string.Empty;

            if (string.IsNullOrEmpty(username))
            {
                error = "username is null";
                return (user, error);
            }

            if (string.IsNullOrEmpty(password))
            {
                error = "password is null";
                return (user, error);
            }

            if (username.Length > MAX_LENGTH_STRING || username.Length < MIN_LENGTH_STRING)
            {
                error = $"need username is {MIN_LENGTH_STRING}-{MAX_LENGTH_STRING} symbols";
                return (user, error);
            }

            if (password.Length > MAX_LENGTH_STRING || password.Length < MIN_LENGTH_STRING)
            {
                error = $"need password is {MIN_LENGTH_STRING}-{MAX_LENGTH_STRING} symbols";
                return (user, error);
            }

            string passwordHash = string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(bytes);
            passwordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            user = new UsersDataForLogin(Guid.NewGuid(), username, passwordHash);
            return (user, error);
        }

        public (UsersDataForLogin? user, string error) Create(Guid id,  string username, string passwordHash)
        {
            UsersDataForLogin? user = null;
            string error = string.Empty;

            if (string.IsNullOrEmpty(username))
            {
                error = "username is null";
                return (user, error);
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                error = "password is null";
                return (user, error);
            }

            if (username.Length > MAX_LENGTH_STRING || username.Length < MIN_LENGTH_STRING)
            {
                error = $"need username is {MIN_LENGTH_STRING}-{MAX_LENGTH_STRING} symbols";
                return (user, error);
            }

            if (passwordHash.Length != 64)
            {
                error = $"need password is hash";
                return (user, error);
            }

            user = new UsersDataForLogin(id, username, passwordHash);
            return (user, error);
        }
    }
}
