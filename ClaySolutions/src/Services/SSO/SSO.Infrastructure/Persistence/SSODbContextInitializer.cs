using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Common;
using SSO.Application.Contracts.Infrastructure;
using SSO.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Infrastructure.Persistence
{
    public class SSODbContextInitializer
    {
        private readonly ILogger<SSODbContextInitializer> _logger;
        private readonly SSODbContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IDateTimeService _dateTimeService;

        public SSODbContextInitializer(ILogger<SSODbContextInitializer> logger, SSODbContext context, IEncryptionService encryptionService, IDateTimeService dateTimeService)
        {
            _logger = logger;
            _context = context;
            _encryptionService = encryptionService;
            _dateTimeService = dateTimeService;
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
            // Default roles
            var adminRole = new Role { Title = SystemRoleDefinition.Admin };
            if (await _context.Roles.AnyAsync() == false)
            {
                _context.Roles.Add(adminRole);
                _context.Roles.Add(new Role { Title = SystemRoleDefinition.Manager });
                _context.Roles.Add(new Role { Title = SystemRoleDefinition.Reporter });
            }

            // Default users
            var admin = new User { FirstName = "Admin", LastName = "Admin", UserName = "admin", Password = _encryptionService.HashPassword("123"), IsActive = true, CreatedDate = _dateTimeService.Now };

            if (_context.Users.All(u => u.UserName != admin.UserName))
            {
                await _context.Users.AddAsync(admin);
                admin.Roles.Add(adminRole);
            }

            await _context.SaveChangesAsync();
        }
    }
}
