using SSO.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSO.Application.Contracts.Persistence
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAsync(string title);
        Task<Role> GetAsync(long id);
        Task<List<Role>> GetByRoleIdsAsync(List<long> roleIds);
    }
}
