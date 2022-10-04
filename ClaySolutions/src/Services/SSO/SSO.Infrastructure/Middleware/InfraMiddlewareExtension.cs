using Microsoft.Extensions.DependencyInjection;
using SSO.Infrastructure.Persistence;
using System;

namespace SSO.Infrastructure.Middleware
{
    public static class InfraMiddlewareExtension
    {
        public async static void DbContextInitializer(this IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<SSODbContextInitializer>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
            }
        }
    }
}
