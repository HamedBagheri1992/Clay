using ClayService.Application.Contracts.Persistence;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Common;
using ClayService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ClayServiceDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ClayServiceDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddOrUpdateAsync(User user)
        {
            try
            {
                await _context.Users.AddOrUpdateAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User added to ClayService Database");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on AddOrUpdate User");
                return false;
            }
        }

        public async Task<User> GetAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserWithPhysicalTagAsync(long id)
        {
            return await _context.Users.Include(u => u.PhysicalTag).AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
