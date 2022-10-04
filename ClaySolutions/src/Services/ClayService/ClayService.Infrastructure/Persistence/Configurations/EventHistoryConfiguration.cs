using ClayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClayService.Infrastructure.Persistence.Configurations
{
    public class EventHistoryConfiguration : IEntityTypeConfiguration<EventHistory>
    {
        public void Configure(EntityTypeBuilder<EventHistory> builder)
        {
            builder.Property(e => e.OperationResult).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.HasIndex(e => new { e.UserId, e.CreatedDate });
            builder.HasIndex(e => new { e.DoorId, e.CreatedDate });
            //builder.HasOne(e => e.Office).WithMany(e => e.EventHistories).HasForeignKey(e => e.OfficeId).OnDelete(DeleteBehavior.NoAction);
            //builder.HasOne(e => e.Door).WithMany(e => e.EventHistories).HasForeignKey(e => e.DoorId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
