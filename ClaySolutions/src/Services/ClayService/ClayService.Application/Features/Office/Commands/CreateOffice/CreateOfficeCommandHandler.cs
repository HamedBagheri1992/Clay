using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Office.Queries.GetOffice;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Commands.CreateOffice
{
    public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, OfficeDto>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public CreateOfficeCommandHandler(IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<OfficeDto> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _officeRepository.CreateAsync(request);
            return _mapper.Map<OfficeDto>(office);
        }
    }
}
