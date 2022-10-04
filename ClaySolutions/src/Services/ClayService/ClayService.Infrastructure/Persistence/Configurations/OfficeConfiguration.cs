using ClayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClayService.Infrastructure.Persistence.Configurations
{
    internal class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.Property(o => o.Title).HasMaxLength(200).IsRequired().IsUnicode();
            builder.Property(o => o.CreatedDate).IsRequired();
        }
    }
}
