using AutoMapper;
using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Door.Queries.GetDoor;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.CreateDoor
{
    public class CreateDoorCommandHandler : IRequestHandler<CreateDoorCommand, DoorDto>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;

        public CreateDoorCommandHandler(IDoorRepository doorRepository, IMapper mapper, IDateTimeService dateTimeService)
        {
            _doorRepository = doorRepository;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public async Task<DoorDto> Handle(CreateDoorCommand request, CancellationToken cancellationToken)
        {
            var door = new Domain.Entities.Door
            {
                Name = request.Name,
                OfficeId = request.OfficeId,
                IsActive = true,
                CreatedDate = _dateTimeService.Now
            };

            var result = await _doorRepository.CreateAsync(door);
            return _mapper.Map<DoorDto>(result);
        }
    }
}
