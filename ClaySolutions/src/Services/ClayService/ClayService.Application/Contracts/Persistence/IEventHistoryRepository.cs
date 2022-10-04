using EventBus.Messages.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IEventHistoryRepository
    {
        Task<bool> BulkInsert(List<EventHistoryCheckoutEvent> eventHistories);
    }
}
