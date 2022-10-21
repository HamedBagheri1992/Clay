using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.UpdateDoor
{
    public class UpdateDoorCommandHandler : IRequestHandler<UpdateDoorCommand>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IMapper _mapper;

        public UpdateDoorCommandHandler(IDoorRepository doorRepository, IMapper mapper)
        {
            _doorRepository = doorRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateDoorCommand request, CancellationToken cancellationToken)
        {
            var doorToUpdate = await _doorRepository.GetAsync(request.Id);
            if (doorToUpdate == null)
                throw new NotFoundException(nameof(Domain.Entities.Door), request.Id);

            _mapper.Map(request, doorToUpdate);

            await _doorRepository.UpdateAsync(doorToUpdate);
            return Unit.Value;
        }
    }
}
