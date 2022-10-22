using ClayService.Application.Contracts.Persistence;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ClayServiceDbContext _context;

        public TagRepository(ClayServiceDbContext context)
        {
            _context = context;
        }

        public async Task<PhysicalTag> GetAsync(long id)
        {
            return await _context.PhysicalTags.FindAsync(id);
        }

        public PhysicalTag GetWithUser(string tagCode)
        {
            return _context.PhysicalTags.Include(p => p.User).AsNoTracking().FirstOrDefault(p => p.TagCode == tagCode);
        }

        public async Task<PaginatedResult<PhysicalTag>> GetAsync(DateTime? startCreatedDate, DateTime? endCreatedDate, string tagCode, int pageNumber, int pageSize)
        {
            var query = _context.PhysicalTags.AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(tagCode) == false)
                query = query.Where(t => t.TagCode.Contains(tagCode));

            if (startCreatedDate.HasValue)
                query = query.Where(t => t.CreatedDate >= startCreatedDate.Value);

            if (endCreatedDate.HasValue)
                query = query.Where(t => t.CreatedDate <= endCreatedDate.Value);

            return await query.ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<PhysicalTag> GetTagsOfUserAsync(long userId)
        {
            return await _context.PhysicalTags.Include(p => p.User).AsNoTracking().FirstOrDefaultAsync(p => p.User.Id == userId);
        }

        public async Task<PhysicalTag> CreateAsync(PhysicalTag tag)
        {
            await _context.PhysicalTags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> IsUniqueTagCodeAsync(string tagCode)
        {
            return await _context.PhysicalTags.AnyAsync(o => o.TagCode == tagCode) == false;
        }
    }
}
