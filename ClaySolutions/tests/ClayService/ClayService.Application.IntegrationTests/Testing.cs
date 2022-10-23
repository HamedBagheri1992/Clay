using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Application.IntegrationTests;

public class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Checkpoint _checkpoint = null!;

    public void RunBeforeAnyTests()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" }
        };
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ClayServiceDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task<TEntity> FirstOrDefaultAsync<TEntity>(long id) where TEntity : EntityBase
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ClayServiceDbContext>();

        return await IncludeAll(context.Set<TEntity>().AsNoTracking()).FirstOrDefaultAsync(t => t.Id == id);
    }

    public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ClayServiceDbContext>();

        context.Set<TEntity>().Add(entity);

        await context.SaveChangesAsync();
    }

    private static IQueryable<T> IncludeAll<T>(IQueryable<T> queryable) where T : EntityBase
    {
        var type = typeof(T);
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            var isVirtual = property.GetGetMethod().IsVirtual;
            if (isVirtual)
            {
                queryable = queryable.Include(property.Name);
            }
        }
        return queryable;
    }
}
