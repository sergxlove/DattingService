namespace DattingService.Core.Models
{
    public class Swipes
    {
        public Guid Id { get; }

        public Guid IdFirstUser { get; }

        public Guid IdSecondUser { get; }

        public bool SolutionFirstUser { get; }

        public bool SolutionSecondUser { get; }
    }
}
