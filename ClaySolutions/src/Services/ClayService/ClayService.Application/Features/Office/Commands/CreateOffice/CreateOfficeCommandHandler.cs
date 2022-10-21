using AutoMapper;
using ClayService.Application.Contracts.Infrastructure;
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
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;

        public CreateOfficeCommandHandler(IOfficeRepository officeRepository, IMapper mapper, IDateTimeService dateTimeService)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public async Task<OfficeDto> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = new Domain.Entities.Office { Title = request.Title, CreatedDate = _dateTimeService.Now };
            var result = await _officeRepository.CreateAsync(office);
            return _mapper.Map<OfficeDto>(result);
        }
    }
}
