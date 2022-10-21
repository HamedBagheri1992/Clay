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
    public class DoorRepository : IDoorRepository
    {
        private readonly ClayServiceDbContext _context;

        public DoorRepository(ClayServiceDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUniqueNameAsync(string name, long officeId)
        {
            return await _context.Doors.AnyAsync(d => d.Name == name && d.OfficeId == officeId) == false;
        }

        public async Task<bool> IsUniqueNameAsync(long id, string name, long officeId)
        {
            return await _context.Doors.AnyAsync(d => d.Name == name && d.OfficeId == officeId && d.Id != id) == false;
        }

        public async Task<Door> GetAsync(long id)
        {
            return await _context.Doors.FindAsync(id);
        }

        public async Task<PaginatedResult<Door>> GetAsync(string name, long? officeId, bool? isActive, int pageNumber, int pageSize)
        {
            var query = _context.Doors.AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(name) == false)
                query = query.Where(d => d.Name.Contains(name));

            if (officeId.HasValue == true)
                query = query.Where(d => d.OfficeId == officeId.Value);


            if (isActive.HasValue == true)
                query = query.Where(d => d.IsActive == isActive.Value);
            else
                query = query.Where(d => d.IsActive == true);

            query = query.OrderBy(q => q.Id);

            return await query.ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<List<Door>> GetDoorsOfUserAsync(long userId)
        {
            return await _context.Doors.AsNoTracking().Where(d => d.Users.Any(u => u.Id == userId)).ToListAsync();
        }

        public async Task<Door> CreateAsync(Door door)
        {
            await _context.Doors.AddAsync(door);
            await _context.SaveChangesAsync();
            return door;
        }

        public async Task UpdateAsync(Door door)
        {
            _context.Entry(door).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }      

        public async Task AssignDoorToUserAsync(Door door, User user)
        {
            door.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsDoorAssignedToUser(long doorId, long userId)
        {
            return await _context.Doors.AnyAsync(o => o.Id == doorId && o.Users.Any(u => u.Id == userId));
        }
    }
}
