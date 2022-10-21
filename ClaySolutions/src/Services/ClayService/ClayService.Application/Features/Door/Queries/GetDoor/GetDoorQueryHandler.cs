using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Queries.GetDoor
{
    public class GetDoorQueryHandler : IRequestHandler<GetDoorQuery, DoorDto>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IMapper _mapper;

        public GetDoorQueryHandler(IDoorRepository doorRepository, IMapper mapper)
        {
            _doorRepository = doorRepository;
            _mapper = mapper;
        }

        public async Task<DoorDto> Handle(GetDoorQuery request, CancellationToken cancellationToken)
        {
            var door = await _doorRepository.GetAsync(request.DoorId);
            if (door is null)
                throw new NotFoundException(nameof(Domain.Entities.Door), request.DoorId);

            return _mapper.Map<DoorDto>(door);
        }
    }
}
