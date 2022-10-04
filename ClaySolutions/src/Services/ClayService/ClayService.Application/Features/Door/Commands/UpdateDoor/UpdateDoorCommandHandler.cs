using ClayService.Application.Contracts.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.UpdateDoor
{
    public class UpdateDoorCommandHandler : IRequestHandler<UpdateDoorCommand>
    {
        private readonly IDoorRepository _doorRepository;

        public UpdateDoorCommandHandler(IDoorRepository doorRepository)
        {
            _doorRepository = doorRepository;
        }

        public async Task<Unit> Handle(UpdateDoorCommand request, CancellationToken cancellationToken)
        {
            await _doorRepository.UpdateAsync(request);
            return Unit.Value;
        }
    }
}
