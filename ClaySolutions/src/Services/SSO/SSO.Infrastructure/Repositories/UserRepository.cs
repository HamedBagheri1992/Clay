using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Extensions;
using SSO.Application.Contracts.Persistence;
using SSO.Domain.Entities;
using SSO.Infrastructure.Persistence;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SSODbContext _context;

        public UserRepository(SSODbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<User>> GetAsync(string userName, string firstName, string lastName, bool? isActive, int pageNumber, int pageSize)
        {
            var users = _context.Users.AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(userName) == false)
                users = users.Where(u => u.UserName.Contains(userName));

            if (string.IsNullOrEmpty(firstName) == false)
                users = users.Where(u => u.FirstName.Contains(firstName));

            if (string.IsNullOrEmpty(lastName) == false)
                users = users.Where(u => u.LastName.Contains(lastName));

            if (isActive.HasValue == true)
                users = users.Where(u => u.IsActive == isActive.Value);

            users = users.OrderBy(u => u.Id);
            return await users.ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            user.IsDeleted = true;
            await UpdateAsync(user);
        }

        public async Task<User> GetUserWithRolesAsync(string userName, string encPass)
        {
            return await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserName == userName && u.Password == encPass);
        }

        public async Task<User> GetUserByPasswordAsync(long userId, string encPass)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Password == encPass);
        }

        public async Task<User> GetUserWithRolesAsync(long id)
        {
            return await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetWithRoleAndRefreshTokensAsync(long userId)
        {
            return await _context.Users.Include(u => u.Roles).Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> IsUniqueUserNameAsync(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName) == false;
        }
    }
}
