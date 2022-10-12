using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Infrastructure
{
    public interface ICacheService
    {
        Task<long> GetUserIdAsync(string tagCode);
        Task<long?> AddOrUpdateTagAsync(string tagCode, long userId);
        long GetUserId(string tagCode);
        Task DeleteTagAsync(string tagCode);
    }
}
