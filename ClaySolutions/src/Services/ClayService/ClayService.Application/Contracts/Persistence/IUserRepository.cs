using ClayService.Domain.Entities;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<bool> AddOrUpdateAsync(User user);
        Task<User> GetAsync(long id);
        Task<User> GetUserWithPhysicalTagAsync(long id);
    }
}
