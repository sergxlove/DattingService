using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Profiles.Postgres.Configurations
{
    public class TempLoginUsersConfigurations : IEntityTypeConfiguration<TempLoginUsersEntity>
    {
        public void Configure(EntityTypeBuilder<TempLoginUsersEntity> builder)
        {
            builder.ToTable("tempLoginUsers");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(LoginUsers.MAX_LENGTH_EMAIL);
            builder.Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(LoginUsers.MAX_LENGTH_PASSWORD);
            builder.HasIndex(a => a.Email)
                .IsUnique();
        }
    }
}
