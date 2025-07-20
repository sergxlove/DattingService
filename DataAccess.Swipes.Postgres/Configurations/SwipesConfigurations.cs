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
        }
    }
}
