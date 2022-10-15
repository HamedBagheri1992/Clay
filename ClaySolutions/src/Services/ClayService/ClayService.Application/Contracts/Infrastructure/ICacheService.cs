using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Infrastructure
{
    public interface ICacheService
    {
        Task InitAsync();
        bool AddOrUpdateTag(string tagCode, long userId);
        long GetUserId(string tagCode);
        void DeleteTag(string tagCode);
    }
}
