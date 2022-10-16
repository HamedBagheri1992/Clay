using ClayService.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.HostedServices
{
    public class CacheServiceInitializer : IHostedService
    {
        private readonly ICacheService _cacheService;

        public CacheServiceInitializer(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {           
            await _cacheService.InitAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
