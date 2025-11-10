using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;

namespace DataAccess.Profiles.Postgres.Configurations
{
    public class UsersConfigurations : IEntityTypeConfiguration<UsersEntity>
    {
        public void Configure(EntityTypeBuilder<UsersEntity> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(Users.MAX_LENGTH_NAME);
            builder.Property(a => a.Age)
                .IsRequired();
            builder.Property(a => a.Target)
                .IsRequired();
            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(Users.MAX_LENGTH_DESCRIPTION);
            builder.Property(a => a.City)
                .IsRequired();
            builder.Property(a => a.PhotoURL)
                .HasColumnType("text[]");
            builder.Property(a => a.IsActive)
                .IsRequired();
            builder.Property(a => a.IsVerify)
                .IsRequired();
        }
    }
}
