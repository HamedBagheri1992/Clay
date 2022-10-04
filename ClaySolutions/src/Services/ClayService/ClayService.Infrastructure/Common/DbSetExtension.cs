using ClayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Common
{
    public static class DbSetExtension
    {
        public static void AddOrUpdate<TEntity>(this DbSet<TEntity> set, TEntity entity) where TEntity : EntityBase
        {
            _ = set.Any(e => e.Id == entity.Id) == false ? set.Add(entity) : set.Update(entity);
        }

        public static async Task AddOrUpdateAsync<TEntity>(this DbSet<TEntity> set, TEntity entity) where TEntity : EntityBase
        {
            _ = await set.AnyAsync(e => e.Id == entity.Id) == false ? await set.AddAsync(entity) : set.Update(entity);
        }

        public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> set, IEnumerable<TEntity> entities) where TEntity : EntityBase
        {
            foreach (var entity in entities)
            {
                _ = set.Any(e => e.Id == entity.Id) == false ? set.Add(entity) : set.Update(entity);
            }
        }

        public static async Task AddOrUpdateRangeAsync<TEntity>(this DbSet<TEntity> set, IEnumerable<TEntity> entities) where TEntity : EntityBase
        {
            foreach (var entity in entities)
            {
                _ = await set.AnyAsync(e => e.Id == entity.Id) == false ? await set.AddAsync(entity) : set.Update(entity);
            }
        }
    }
}
