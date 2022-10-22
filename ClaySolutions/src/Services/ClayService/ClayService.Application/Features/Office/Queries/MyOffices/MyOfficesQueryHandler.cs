using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Office.Queries.GetOffice;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Queries.MyOffices
{
    public class MyOfficesQueryHandler : IRequestHandler<MyOfficesQuery, List<OfficeDto>>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public MyOfficesQueryHandler(IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<List<OfficeDto>> Handle(MyOfficesQuery request, CancellationToken cancellationToken)
        {
            var offices = await _officeRepository.GetAsync(request.UserId);
            return _mapper.Map<List<OfficeDto>>(offices);
        }
    }
}
