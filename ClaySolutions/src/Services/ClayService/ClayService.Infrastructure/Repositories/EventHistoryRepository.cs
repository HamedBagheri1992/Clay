using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.EventHistory.Queries.GetEventHistories;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using EFCore.BulkExtensions;
using SharedKernel.Common;
using SharedKernel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Repositories
{
    public class EventHistoryRepository : IEventHistoryRepository
    {
        private readonly ClayServiceDbContext _context;

        public EventHistoryRepository(ClayServiceDbContext context)
        {
            _context = context;
        }

        public async Task BulkInsert(List<EventHistory> eventHistories)
        {
            await _context.BulkInsertAsync(eventHistories);
        }

        public async Task<PaginatedResult<EventHistoryDto>> GetAsync(DateTime startCreatedDate, DateTime endCreatedDate, long? userId, long? doorId, int pageNumber, int pageSize)
        {
            var query = from EH in _context.Set<EventHistory>()
                        from D in _context.Set<Door>().Where(D => D.Id == EH.DoorId)
                        from O in _context.Set<Office>().Where(O => O.Id == D.OfficeId)
                        from U in _context.Set<User>().Where(U => U.Id == EH.UserId)
                        select new EventHistoryDto
                        {
                            Id = EH.Id,
                            TagCode = EH.TagCode,
                            CreatedDate = EH.CreatedDate,
                            SourceType = EH.SourceType,
                            OperationResult = EH.OperationResult,
                            DoorId = EH.DoorId,
                            DoorName = D.Name,
                            OfficeId = EH.OfficeId,
                            OfficeName = O.Title,
                            UserId = U.Id,
                            UserDisplayName = U.DisplayName,
                            UserName = U.UserName
                        };

            query = query.Where(q => q.CreatedDate >= startCreatedDate && q.CreatedDate <= endCreatedDate);

            if (userId.HasValue == true)
                query = query.Where(e => e.UserId == userId.Value);

            if (doorId.HasValue == true)
                query = query.Where(e => e.DoorId == doorId.Value);

            return await query.ToPagedListAsync(pageNumber, pageSize);
        }
    }
}
