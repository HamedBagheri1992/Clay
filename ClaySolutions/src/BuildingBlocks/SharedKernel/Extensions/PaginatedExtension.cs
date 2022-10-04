using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SharedKernel.Extensions
{
    public static class PaginatedExtension
    {
        public static PaginatedResult<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedResult<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PaginatedResult<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
