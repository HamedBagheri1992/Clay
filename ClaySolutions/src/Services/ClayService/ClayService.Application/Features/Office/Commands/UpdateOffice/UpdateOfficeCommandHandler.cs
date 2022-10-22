using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Commands.UpdateOffice
{
    public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public UpdateOfficeCommandHandler(IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            var officeToUpdate = await _officeRepository.GetAsync(request.Id);
            if (officeToUpdate == null)
                throw new NotFoundException(nameof(Domain.Entities.Office), request.Id);

            _mapper.Map(request, officeToUpdate);

            await _officeRepository.UpdateAsync(officeToUpdate);
            return Unit.Value;
        }
    }
}
