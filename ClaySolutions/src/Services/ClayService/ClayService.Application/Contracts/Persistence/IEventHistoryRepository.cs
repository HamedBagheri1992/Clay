using ClayService.Application.Features.EventHistory.Queries.GetEventHistories;
using ClayService.Domain.Entities;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IEventHistoryRepository
    {
        Task BulkInsert(List<EventHistory> eventHistories);
        Task<PaginatedResult<EventHistoryDto>> GetAsync(DateTime startCreatedDate, DateTime endCreatedDate, long? userId, long? doorId, int pageNumber, int pageSize);
    }
}
