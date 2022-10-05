using ClayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClayService.Infrastructure.Persistence
{
    public class ClayServiceDbContext : DbContext
    {
        public ClayServiceDbContext()
        {

        }

        public ClayServiceDbContext(DbContextOptions<ClayServiceDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Office> offices { get; set; }
        public virtual DbSet<Door> Doors { get; set; }
        public virtual DbSet<EventHistory> EventHistories { get; set; }
        public virtual DbSet<PhysicalTag> PhysicalTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
