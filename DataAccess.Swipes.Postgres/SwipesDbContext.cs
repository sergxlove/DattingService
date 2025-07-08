using Microsoft.EntityFrameworkCore;

namespace DataAccess.Swipes.Postgres
{
    public class SwipesDbContext : DbContext
    {
        public SwipesDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
