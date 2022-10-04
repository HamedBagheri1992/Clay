using ClayService.Domain.Entities;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<bool> AddOrUpdateAsync(User user);
    }
}
