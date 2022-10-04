using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.OperationDoor
{
    public class OperationDoorCommandHandler : IRequestHandler<OperationDoorCommand>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IMapper _mapper;

        public OperationDoorCommandHandler(IDoorRepository doorRepository, IMapper mapper)
        {
            _doorRepository = doorRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(OperationDoorCommand request, CancellationToken cancellationToken)
        {
            await _doorRepository.OperationAsync(request);
            return Unit.Value;
        }
    }
}
