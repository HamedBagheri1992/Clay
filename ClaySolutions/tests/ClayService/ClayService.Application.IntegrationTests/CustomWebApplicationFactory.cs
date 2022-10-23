using ClayService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClayService.Application.IntegrationTests;
internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {
            //services
            //    .Remove<ICurrentUserService>()
            //    .AddTransient(provider => Mock.Of<ICurrentUserService>(s =>
            //        s.UserId == GetCurrentUserId()));

            services
                .Remove<DbContextOptions<ClayServiceDbContext>>()
                .AddDbContext<ClayServiceDbContext>((sp, options) =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsAssembly(typeof(ClayServiceDbContext).Assembly.FullName)));
        });
    }
}
