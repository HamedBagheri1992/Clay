using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Office.Commands.CreateOffice;
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
        private readonly IDateTimeService _dateTimeService;

        public OfficeRepository(ClayServiceDbContext context, IDateTimeService dateTimeService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
        }

        public async Task<Office> GetAsync(GetOfficeQuery request)
        {
            var office = await _context.offices.AsNoTracking().FirstOrDefaultAsync(d => d.Id == request.OfficeId);
            if (office == null)
                throw new NotFoundException(nameof(office), request.OfficeId);

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

        public async Task<Office> CreateAsync(CreateOfficeCommand request)
        {
            var office = new Office()
            {
                Title = request.Title,
                IsDeleted = false,
                CreatedDate = _dateTimeService.Now
            };

            await _context.offices.AddAsync(office);
            await _context.SaveChangesAsync();

            return office;
        }
    }
}
