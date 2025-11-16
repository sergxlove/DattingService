using DataAccess.Profiles.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Profiles.Postgres.Configurations
{
    public class TokensUserConfigurations : IEntityTypeConfiguration<TokensUserEntity>
    {
        public void Configure(EntityTypeBuilder<TokensUserEntity> builder)
        {
            builder.ToTable("tokensUser");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.UserId)
                .IsRequired();
            builder.Property(t => t.Created)
                .IsRequired();
            builder.Property(t => t.Ended)
                .IsRequired();
            builder.Property(t => t.Role)
                .IsRequired();
        }
    }
}
