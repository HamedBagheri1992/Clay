using ClayService.Domain.Entities;
using SharedKernel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IDoorRepository
    {
        Task<Door> CreateAsync(Door door);
        Task<Door> GetAsync(long id);
        Task<PaginatedResult<Door>> GetAsync(string name, long? officeId, bool? isActive, int pageNumber, int pageSize);
        Task<List<Door>> GetDoorsOfUserAsync(long userId);
        Task<bool> IsUniqueNameAsync(string name, long officeId);
        Task<bool> IsUniqueNameAsync(long id, string name, long officeId);
        Task UpdateAsync(Door door);
        Task AssignDoorToUserAsync(Door door, User user);
        Task<bool> IsDoorAssignedToUser(long doorId, long userId);
    }
}
