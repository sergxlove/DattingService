using DattingService.Core.Abstractions;

namespace DattingService.Infrastructure.Services
{
    public class PasswordValidatorService : IPasswordValidatorService
    {
        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (password.Length < 8) return false;
            if (!password.Any(char.IsDigit)) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsLower)) return false;
            if (!password.Any(c => !char.IsLetterOrDigit(c))) return false;
            return true;
        }

        public int ScorePassword(string password)
        {
            int score = 0;
            if (password.Length >= 8) score += 20;
            if (password.Length >= 12) score += 20;
            if (password.Any(char.IsDigit)) score += 15;
            if (password.Any(char.IsUpper)) score += 15;
            if (password.Any(char.IsLower)) score += 15;
            if (password.Any(c => !char.IsLetterOrDigit(c))) score += 15;
            return score;
        }
    }
}
