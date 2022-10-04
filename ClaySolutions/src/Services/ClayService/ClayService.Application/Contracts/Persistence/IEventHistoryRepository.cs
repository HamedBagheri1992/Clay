using ClayService.Application.Features.EventHistory.Queries.GetEventHistories;
using ClayService.Domain.Entities;
using EventBus.Messages.Events;
using SharedKernel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IEventHistoryRepository
    {
        Task<bool> BulkInsert(List<EventHistoryCheckoutEvent> eventHistories);
        Task<PaginatedResult<EventHistoryDto>> GetAsync(GetEventHistoriesQuery request);
    }
}
