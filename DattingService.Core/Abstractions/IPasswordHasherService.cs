namespace DattingService.Core.Abstractions
{
    public interface IPasswordHasherService
    {
        public string Hash(string password);
        public bool Verify(string password, string hashedPassword);
    }

    public enum HashAlgorithm
    {
        PBKDF2 = 1,
        BCrypt = 2
    }
}
