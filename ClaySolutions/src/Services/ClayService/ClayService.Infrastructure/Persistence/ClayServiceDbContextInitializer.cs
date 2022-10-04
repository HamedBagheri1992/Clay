using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using ClayService.Domain.Entities;
using System.Linq;

namespace ClayService.Infrastructure.Persistence
{
    public class ClayServiceDbContextInitializer
    {
        private readonly ILogger<ClayServiceDbContextInitializer> _logger;
        private readonly ClayServiceDbContext _context;

        public ClayServiceDbContextInitializer(ILogger<ClayServiceDbContextInitializer> logger, ClayServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Default users
            var admin = new User { Id = 1, UserName = "admin", DisplayName = "Admin" };

            if (_context.Users.All(u => u.UserName != admin.UserName))
            {
                await _context.Users.AddAsync(admin);
            }

            await _context.SaveChangesAsync();
        }
    }
}
