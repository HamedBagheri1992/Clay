using Microsoft.EntityFrameworkCore;
using SSO.Application.Contracts.Persistence;
using SSO.Domain.Entities;
using SSO.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SSODbContext _context;

        public RoleRepository(SSODbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetAsync(long id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<List<Role>> GetAsync(string title)
        {
            var roles = _context.Roles.AsNoTracking().AsQueryable();
            if (string.IsNullOrEmpty(title) == false)
                roles = roles.Where(r => r.Title.Contains(title));

            roles = roles.OrderBy(r => r.Id);
            return await roles.ToListAsync();
        }

        public async Task<List<Role>> GetByRoleIdsAsync(List<long> roleIds)
        {
            return await _context.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
        }
    }
}
