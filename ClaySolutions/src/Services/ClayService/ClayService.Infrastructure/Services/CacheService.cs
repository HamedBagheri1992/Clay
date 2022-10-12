using ClayService.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Services
{
    public class CacheService : ICacheService
    {

        private readonly IDistributedCache _redisCache;

        public CacheService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<long> GetUserIdAsync(string tagCode)
        {
            var value = await _redisCache.GetStringAsync(tagCode);
            if (long.TryParse(value, out long userId) == false)
                return 0;

            return userId;
        }

        public long GetUserId(string tagCode)
        {
            var value = _redisCache.GetString(tagCode);
            if (long.TryParse(value, out long userId) == false)
                return 0;

            return userId;
        }

        public async Task<long?> AddOrUpdateTagAsync(string tagCode, long userId)
        {
            await _redisCache.SetStringAsync(tagCode, userId.ToString());
            return await GetUserIdAsync(tagCode);
        }

        public async Task DeleteTagAsync(string tagCode)
        {
            await _redisCache.RemoveAsync(tagCode);
        }
    }
}
