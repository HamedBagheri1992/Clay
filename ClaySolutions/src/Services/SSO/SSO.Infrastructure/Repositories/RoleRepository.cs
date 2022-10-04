using Microsoft.EntityFrameworkCore;
using SharedKernel.Exceptions;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.Role.Queries.GetRole;
using SSO.Application.Features.Role.Queries.GetRoles;
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

        public async Task<Role> GetAsync(GetRoleQuery request)
        {
            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == request.Id);
            if (role == null)
                throw new NotFoundException(nameof(role), request.Id);

            return role;
        }

        public async Task<List<Role>> GetAsync(GetRolesQuery request)
        {
            var roles = _context.Roles.AsNoTracking().AsQueryable();
            if (string.IsNullOrEmpty(request.Title) == false)
                roles = roles.Where(r => r.Title.Contains(request.Title));

            return await roles.ToListAsync();
        }
    }
}
