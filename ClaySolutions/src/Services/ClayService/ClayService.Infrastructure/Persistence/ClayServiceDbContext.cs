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

        public DbSet<User> Users { get; set; }
        public DbSet<Office> offices { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<EventHistory> EventHistories { get; set; }
        public DbSet<PhysicalTag> PhysicalTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
