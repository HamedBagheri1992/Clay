using MediatR;
using SharedKernel.Common;
using System;

namespace ClayService.Application.Features.EventHistory.Queries.GetEventHistories
{
    public class GetEventHistoriesQuery : PaginationQuery, IRequest<PaginatedList<EventHistoryDto>>
    {
        public long? UserId { get; set; }
        public long? DoorId { get; set; }
        public DateTime StartCreatedDate { get; set; }
        public DateTime EndCreatedDate { get; set; }
    }
}
