using ClayService.Application.Contracts.Persistence;
using MediatR;
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
            await _officeRepository.DeleteAsync(request);
            return Unit.Value;
        }
    }
}
