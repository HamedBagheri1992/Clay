using Microsoft.EntityFrameworkCore;
using SSO.Domain.Entities;
using System.Reflection;

namespace SSO.Infrastructure.Persistence
{
    public class SSODbContext : DbContext
    {
        public SSODbContext()
        {

        }

        public SSODbContext(DbContextOptions<SSODbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
