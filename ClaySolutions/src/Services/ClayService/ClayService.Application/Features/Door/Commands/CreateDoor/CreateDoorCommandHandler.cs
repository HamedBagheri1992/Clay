using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Door.Queries.GetDoor;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.CreateDoor
{
    public class CreateDoorCommandHandler : IRequestHandler<CreateDoorCommand, DoorDto>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IMapper _mapper;

        public CreateDoorCommandHandler(IDoorRepository doorRepository, IMapper mapper)
        {
            _doorRepository = doorRepository;
            _mapper = mapper;
        }

        public async Task<DoorDto> Handle(CreateDoorCommand request, CancellationToken cancellationToken)
        {
            var door = await _doorRepository.CreateAsync(request);
            return _mapper.Map<DoorDto>(door);
        }
    }
}
