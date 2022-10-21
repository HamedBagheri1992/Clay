using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Door.Queries.GetDoor;
using MediatR;
using SharedKernel.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Queries.GetDoors
{
    public class GetDoorsQueryHandler : IRequestHandler<GetDoorsQuery, PaginatedList<DoorDto>>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IMapper _mapper;

        public GetDoorsQueryHandler(IDoorRepository doorRepository, IMapper mapper)
        {
            _doorRepository = doorRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<DoorDto>> Handle(GetDoorsQuery request, CancellationToken cancellationToken)
        {
            var doors = await _doorRepository.GetAsync(request.Name, request.OfficeId, request.IsActive, request.PageNumber, request.PageSize);
            return _mapper.Map<PaginatedList<DoorDto>>(doors);
        }
    }
}
