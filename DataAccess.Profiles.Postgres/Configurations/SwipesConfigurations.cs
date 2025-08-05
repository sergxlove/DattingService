using DataAccess.Swipes.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Swipes.Postgres.Configurations
{
    public class SwipesConfigurations : IEntityTypeConfiguration<SwipesEntity>
    {
        public void Configure(EntityTypeBuilder<SwipesEntity> builder)
        {
            builder.ToTable("swipes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IdFirstUser)
                .IsRequired();
            builder.Property(x => x.IdSecondUser)
                .IsRequired();
            builder.Property(x => x.SolutionFirstUser)
                .IsRequired();
            builder.Property(x => x.SolutionSecondUser)
                .IsRequired();
        }
    }
}
