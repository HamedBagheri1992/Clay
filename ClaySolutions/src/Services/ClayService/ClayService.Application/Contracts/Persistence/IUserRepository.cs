using ClayService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<User> AddOrUpdateAsync(User user);
        Task<User> GetAsync(long id);
        Task<User> GetUserWithPhysicalTagAsync(long id);
        Task AssignTagAsync(User user);
        IEnumerable<User> GetUsersWithPhysicalTag();
    }
}
