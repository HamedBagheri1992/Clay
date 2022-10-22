using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.EventHistory.Queries.GetEventHistories
{
    public class GetEventHistoriesQueryHandler : IRequestHandler<GetEventHistoriesQuery, PaginatedList<EventHistoryDto>>
    {
        private readonly IEventHistoryRepository _eventHistoryRepository;
        private readonly IMapper _mapper;

        public GetEventHistoriesQueryHandler(IEventHistoryRepository eventHistoryRepository, IMapper mapper)
        {
            _eventHistoryRepository = eventHistoryRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<EventHistoryDto>> Handle(GetEventHistoriesQuery request, CancellationToken cancellationToken)
        {
            var events = await _eventHistoryRepository.GetAsync(request.StartCreatedDate, request.EndCreatedDate, request.UserId, request.DoorId, request.PageNumber, request.PageSize);
            return _mapper.Map<PaginatedList<EventHistoryDto>>(events);
        }
    }
}
