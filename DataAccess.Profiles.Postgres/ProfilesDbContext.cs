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
        public DbSet<UserDataForLoginEntity> UsersLogin { get; set; }
        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<SwipesEntity> Swipes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfigurations());
            modelBuilder.ApplyConfiguration(new UsersDataForLoginEntityConfigurations());
            modelBuilder.ApplyConfiguration(new SwipesConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}
