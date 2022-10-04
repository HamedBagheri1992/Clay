using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Queries.GetOffice
{
    public class GetOfficeQueryHandler : IRequestHandler<GetOfficeQuery, OfficeDto>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public GetOfficeQueryHandler(IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<OfficeDto> Handle(GetOfficeQuery request, CancellationToken cancellationToken)
        {
            var office = await _officeRepository.GetAsync(request);
            return _mapper.Map<OfficeDto>(office);
        }
    }
}
