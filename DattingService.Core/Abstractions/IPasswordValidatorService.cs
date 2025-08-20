namespace DattingService.Core.Abstractions
{
    public interface IPasswordValidatorService
    {
        public bool IsValidPassword(string password);
        public int ScorePassword(string password);
    }
}
