using ClayService.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.AssignDoor
{
    public class AssignDoorCommandHandler : IRequestHandler<AssignDoorCommand>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOfficeRepository _officeRepository;

        public AssignDoorCommandHandler(IDoorRepository doorRepository, IUserRepository userRepository, IOfficeRepository officeRepository)
        {
            _doorRepository = doorRepository;
            _userRepository = userRepository;
            _officeRepository = officeRepository;
        }

        public async Task<Unit> Handle(AssignDoorCommand request, CancellationToken cancellationToken)
        {
            var door = await _doorRepository.GetWithOfficeAsync(request.DoorId);
            if (door == null)
                throw new NotFoundException(nameof(Domain.Entities.Door), request.DoorId);

            var user = await _userRepository.GetAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(Domain.Entities.User), request.UserId);

            if (request.IsAdmin == false)
                if (await _officeRepository.IsOfficeAssignedToUser(door.OfficeId, request.CurrentUserId) == false)
                    throw new BadRequestException("office access denied");

            await _officeRepository.AssignOfficeToUserAsync(door.Office, user);
            await _doorRepository.AssignDoorToUserAsync(door, user);
            return Unit.Value;
        }
    }
}
