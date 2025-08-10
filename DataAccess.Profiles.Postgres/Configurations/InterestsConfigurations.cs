using DataAccess.Profiles.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;

namespace DataAccess.Profiles.Postgres.Configurations
{
    public class InterestsConfigurations : IEntityTypeConfiguration<InterestsEntity>
    {
        public void Configure(EntityTypeBuilder<InterestsEntity> builder)
        {
            builder.ToTable("iterests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SelectInterests)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => v.ToString(),
                    v => JArray.Parse(v ?? "[]"));
        }
    }
}
