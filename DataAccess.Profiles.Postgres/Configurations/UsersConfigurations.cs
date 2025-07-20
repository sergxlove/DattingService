using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Profiles.Postgres.Configurations
{
    public class UsersConfigurations : IEntityTypeConfiguration<UsersEntity>
    {
        public void Configure(EntityTypeBuilder<UsersEntity> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired()
                .HasMaxLength(Users.MAX_LENGTH_STRING);
            builder.Property(x => x.Age).IsRequired();
            builder.Property(x => x.City).IsRequired()
                .HasMaxLength(Users.MAX_LENGTH_STRING);
            builder.Property(x => x.Description).IsRequired()
                .HasMaxLength(Users.MAX_LENGTH_DESCRIPTION);
            builder.Property(x => x.IsActive).IsRequired();
        }
    }
}
