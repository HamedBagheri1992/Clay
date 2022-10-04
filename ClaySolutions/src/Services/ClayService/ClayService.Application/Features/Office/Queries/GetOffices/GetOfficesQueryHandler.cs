using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Office.Queries.GetOffice;
using MediatR;
using SharedKernel.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Queries.GetOffices
{
    public class GetOfficesQueryHandler : IRequestHandler<GetOfficesQuery, PaginatedList<OfficeDto>>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public GetOfficesQueryHandler(IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<OfficeDto>> Handle(GetOfficesQuery request, CancellationToken cancellationToken)
        {
            var offices = await _officeRepository.GetAsync(request);
            return _mapper.Map<PaginatedList<OfficeDto>>(offices);
        }
    }
}
