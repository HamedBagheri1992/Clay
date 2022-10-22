using ClayService.Application.Contracts.Persistence;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Common;
using ClayService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ClayServiceDbContext _context;

        public UserRepository(ClayServiceDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddOrUpdateAsync(User user)
        {
            await _context.Users.AddOrUpdateAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserWithPhysicalTagAsync(long id)
        {
            return await _context.Users.Include(u => u.PhysicalTag).AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AssignTagAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<User> GetUsersWithPhysicalTag()
        {
            return _context.Users.Include(u => u.PhysicalTag).AsNoTracking().Where(u => u.PhysicalTagId.HasValue == true).AsEnumerable();
        }
    }
}
