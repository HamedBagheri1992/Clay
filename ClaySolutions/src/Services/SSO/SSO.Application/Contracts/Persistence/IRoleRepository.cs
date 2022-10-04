using SSO.Application.Features.Role.Queries.GetRole;
using SSO.Application.Features.Role.Queries.GetRoles;
using SSO.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSO.Application.Contracts.Persistence
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAsync(GetRolesQuery request);
        Task<Role> GetAsync(GetRoleQuery request);
    }
}
