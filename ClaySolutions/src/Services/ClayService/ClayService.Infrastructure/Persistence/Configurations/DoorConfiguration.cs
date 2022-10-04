using ClayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClayService.Infrastructure.Persistence.Configurations
{
    public class DoorConfiguration : IEntityTypeConfiguration<Door>
    {
        public void Configure(EntityTypeBuilder<Door> builder)
        {
            builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
            builder.Property(d => d.IsActive).IsRequired();
            builder.Property(d => d.CreatedDate).IsRequired();
        }
    }
}
