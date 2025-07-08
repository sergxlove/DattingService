using DataAccess.Profiles.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Profiles.Postgres
{
    public class ProfilesDbContext : DbContext
    {
        public ProfilesDbContext()
        {
            
        }

        public DbSet<UsersEntity> Users { get; set; }

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
