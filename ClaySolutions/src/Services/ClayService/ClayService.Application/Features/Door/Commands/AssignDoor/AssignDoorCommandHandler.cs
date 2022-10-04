using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.AssignDoor
{
    public class AssignDoorCommandHandler : IRequestHandler<AssignDoorCommand>
    {
        private readonly IDoorRepository _doorRepository;

        public AssignDoorCommandHandler(IDoorRepository doorRepository)
        {
            _doorRepository = doorRepository;
        }

        public async Task<Unit> Handle(AssignDoorCommand request, CancellationToken cancellationToken)
        {
            await _doorRepository.AssignDoorToUserAsync(request);
            return Unit.Value;
        }
    }
}
