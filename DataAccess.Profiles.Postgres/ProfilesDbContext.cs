using DataAccess.Profiles.Postgres.Configurations;
using DataAccess.Profiles.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Profiles.Postgres
{
    public class ProfilesDbContext : DbContext
    {
        public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options) { }
        public DbSet<UserDataForLoginEntity> UsersLogin { get; set; }
        public DbSet<UsersEntity> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfigurations());
            modelBuilder.ApplyConfiguration(new UsersDataForLoginEntityConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}
