using DattingService.Core.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace DattingService.Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly Core.Abstractions.HashAlgorithm _algorithmType;
        private readonly int _iteration;
        private readonly int _saltSize;
        private readonly int _hashSize;

        public PasswordHasherService(Core.Abstractions.HashAlgorithm algorithmType, 
            int iteration, int saltSize, int hashSize)
        {
            _algorithmType = algorithmType;
            _iteration = iteration;
            _saltSize = saltSize;
            _hashSize = hashSize;
        }

        public PasswordHasherService()
        {
            _algorithmType = Core.Abstractions.HashAlgorithm.PBKDF2;
            _iteration = 1000;
            _saltSize = 16;
            _hashSize = 32;
        }

        public string Hash(string password)
        {
            switch(_algorithmType)
            {
                case Core.Abstractions.HashAlgorithm.PBKDF2:
                    return HashPBKDF2(password);
                case Core.Abstractions.HashAlgorithm.BCrypt:
                    return HashBCrypt(password);
                default:
                    return string.Empty;
            }
        }

        public bool Verify(string password, string hashedPassword)
        {
            switch(_algorithmType)
            {
                case Core.Abstractions.HashAlgorithm.PBKDF2:
                    return VerifyPBKDF2(password, hashedPassword);
                case Core.Abstractions.HashAlgorithm.BCrypt:
                    return VerifyBCrypt(password, hashedPassword);
                default:
                    return false;
            }
        }

        private string HashPBKDF2(string password)
        {
            byte[] salt = new byte[_saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iteration,
                numBytesRequested: _hashSize);
            byte[] hashBytes = new byte[_hashSize + _saltSize];
            Array.Copy(salt, 0, hashBytes, 0, _saltSize);
            Array.Copy(hash, 0, hashBytes, _saltSize, _hashSize);
            return Convert.ToBase64String(hashBytes);
        }

        private string HashBCrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPBKDF2(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            if (hashBytes.Length != _saltSize + _hashSize)
                return false;
            byte[] salt = new byte[_saltSize];
            Array.Copy(hashBytes, 0, salt, 0, _saltSize);
            byte[] hash = new byte[_hashSize];
            Array.Copy(hashBytes, _saltSize, hash, 0, _hashSize);
            byte[] actualHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iteration,
                numBytesRequested: _hashSize);
            return CryptographicOperations.FixedTimeEquals(actualHash, hash);
        }

        private bool VerifyBCrypt(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
