using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Profiles.Postgres.Configurations
{
    public class UsersDataForLoginEntityConfigurations : IEntityTypeConfiguration<UserDataForLoginEntity>
    {
        public void Configure(EntityTypeBuilder<UserDataForLoginEntity> builder)
        {
            builder.ToTable("usersLogin");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Username)
                .IsRequired()
                .HasMaxLength(UsersDataForLogin.MAX_LENGTH_STRING);
            builder.Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(UsersDataForLogin.MAX_LENGTH_STRING);
        }
    }
}
