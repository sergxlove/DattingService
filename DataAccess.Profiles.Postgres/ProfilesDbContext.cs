using DataAccess.Profiles.Postgres.Configurations;
using DataAccess.Profiles.Postgres.Models;
using DataAccess.Swipes.Postgres.Configurations;
using DataAccess.Swipes.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Profiles.Postgres
{
    public class ProfilesDbContext : DbContext
    {
        public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options) { }
        public DbSet<LoginUsersEntity> LoginUsers { get; set; }
        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<InterestsEntity> Interests { get; set; }
        public DbSet<SwipesEntity> Swipes { get; set; }
        public DbSet<TempLoginUsersEntity> TempLoginUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfigurations());
            modelBuilder.ApplyConfiguration(new LoginUsersConfigurations());
            modelBuilder.ApplyConfiguration(new InterestsConfigurations());
            modelBuilder.ApplyConfiguration(new SwipesConfigurations());
            modelBuilder.ApplyConfiguration(new TempLoginUsersConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}
