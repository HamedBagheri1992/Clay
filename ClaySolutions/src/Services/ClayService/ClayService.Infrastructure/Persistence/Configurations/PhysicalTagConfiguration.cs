using ClayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClayService.Infrastructure.Persistence.Configurations
{
    public class PhysicalTagConfiguration : IEntityTypeConfiguration<PhysicalTag>
    {
        public void Configure(EntityTypeBuilder<PhysicalTag> builder)
        {
            builder.HasIndex(t => new { t.TagCode, t.CreatedDate });
            builder.HasIndex(t => t.TagCode).IsUnique();
        }
    }
}
