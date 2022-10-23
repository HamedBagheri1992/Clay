using ClayService.Application.Contracts.Persistence;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly ClayServiceDbContext _context;

        public OfficeRepository(ClayServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Office> GetAsync(long officeId)
        {
            return await _context.offices.FindAsync(officeId);
        }

        public async Task<PaginatedResult<Office>> GetAsync(string title, int pageNumber, int pageSize)
        {
            var query = _context.offices.AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(title) == false)
                query = query.Where(d => d.Title.Contains(title));

            query = query.OrderBy(q => q.Id);
            return await query.ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<List<Office>> GetOfficesOfUserAsync(long userId)
        {
            return await _context.offices.AsNoTracking().Where(d => d.Users.Any(u => u.Id == userId)).ToListAsync();
        }

        public async Task<Office> CreateAsync(Office office)
        {
            await _context.offices.AddAsync(office);
            await _context.SaveChangesAsync();
            return office;
        }

        public async Task UpdateAsync(Office office)
        {
            _context.Entry(office).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Office office)
        {
            office.IsDeleted = true;
            await UpdateAsync(office);
        }

        public async Task<bool> IsUniqueTitleAsync(string title)
        {
            return await _context.offices.AnyAsync(o => o.Title == title) == false;
        }

        public async Task<bool> IsUniqueTitleAsync(long id, string title)
        {
            return await _context.offices.AnyAsync(o => o.Title == title && o.Id != id) == false;
        }

        public async Task AssignOfficeToUserAsync(Office office, User user)
        {
            office.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsOfficeAssignedToUser(long officeId, long userId)
        {
            return await _context.offices.AnyAsync(o => o.Id == officeId && o.Users.Any(u => u.Id == userId));
        }
    }
}
