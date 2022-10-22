using ClayService.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Commands.DeleteOffice
{
    public class DeleteOfficeCommandHandler : IRequestHandler<DeleteOfficeCommand>
    {
        private readonly IOfficeRepository _officeRepository;

        public DeleteOfficeCommandHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<Unit> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _officeRepository.GetAsync(request.Id);
            if (office == null)
                throw new NotFoundException(nameof(Domain.Entities.Office), request.Id);

            await _officeRepository.DeleteAsync(office);
            return Unit.Value;
        }
    }
}
