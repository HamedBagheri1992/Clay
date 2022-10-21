using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Office.Commands.DeleteOffice;
using ClayService.Application.Features.Office.Commands.UpdateOffice;
using ClayService.Application.Features.Office.Queries.GetOffice;
using ClayService.Application.Features.Office.Queries.GetOffices;
using ClayService.Application.Features.Office.Queries.MyOffices;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Exceptions;
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

        public async Task<Office> GetAsync(GetOfficeQuery request)
        {
            var office = await _context.offices.AsNoTracking().FirstOrDefaultAsync(d => d.Id == request.Id);
            if (office == null)
                throw new NotFoundException(nameof(office), request.Id);

            return office;
        }

        public async Task<PaginatedResult<Office>> GetAsync(GetOfficesQuery request)
        {
            var query = _context.offices.AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(request.Title) == false)
                query = query.Where(d => d.Title.Contains(request.Title));

            return await query.ToPagedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<List<Office>> GetAsync(MyOfficesQuery request)
        {
            return await _context.Users.Include(u => u.Offices).AsNoTracking().Where(u => u.Id == request.UserId).SelectMany(o => o.Offices).ToListAsync();
        }

        public async Task<Office> CreateAsync(Office office)
        {
            await _context.offices.AddAsync(office);
            await _context.SaveChangesAsync();
            return office;
        }

        public async Task UpdateAsync(UpdateOfficeCommand request)
        {
            var office = await _context.offices.FindAsync(request.Id);
            if (office == null)
                throw new NotFoundException(nameof(office), request.Id);

            if (await _context.offices.AnyAsync(o => o.Title == request.Title && o.Id != request.Id) == true)
                throw new BadRequestException("Office name is duplicate");

            office.Title = request.Title;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteOfficeCommand request)
        {
            var office = await _context.offices.FindAsync(request.Id);
            if (office == null)
                throw new NotFoundException(nameof(office), request.Id);

            office.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUniqueTitleAsync(string title)
        {
            return await _context.offices.AnyAsync(o => o.Title == title) == false;
        }

        public async Task<bool> IsUniqueTitleAsync(long id, string title)
        {
            return await _context.offices.AnyAsync(o => o.Title == title && o.Id != id) == false;
        }

        public async Task AssignOfficeToUserAsync(long officeId, User user)
        {
            var office = await _context.offices.FindAsync(officeId);
            if (office.Users.Any(u => u.Id == user.Id) == false)
            {
                office.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsOfficeAssignedToUser(long officeId, long userId)
        {
            return await _context.offices.AnyAsync(o => o.Id == officeId && o.Users.Any(u => u.Id == userId));
        }
    }
}
