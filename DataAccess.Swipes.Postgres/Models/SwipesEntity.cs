namespace DataAccess.Swipes.Postgres.Models
{
    public class SwipesEntity
    {
        public Guid Id { get; set; }

        public Guid IdFirstUser { get; set; }

        public Guid IdSecondUser { get; set; }

        public bool SolutionFirstUser { get; set; }

        public bool SolutionSecondUser { get; set; }
    }
}
