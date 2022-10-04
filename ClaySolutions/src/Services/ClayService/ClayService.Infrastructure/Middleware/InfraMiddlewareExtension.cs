using ClayService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ClayService.Infrastructure.Middleware
{
    public static class InfraMiddlewareExtension
    {
        public async static void DbContextInitializer(this IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<ClayServiceDbContextInitializer>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
            }
        }
    }
}
