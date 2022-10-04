using ClayService.Application.Contracts.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Commands.UpdateOffice
{
    public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand>
    {
        private readonly IOfficeRepository _officeRepository;

        public UpdateOfficeCommandHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<Unit> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            await _officeRepository.UpdateAsync(request);
            return Unit.Value;
        }
    }
}
