using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Door.Queries.GetDoor;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Queries.MyDoors
{
    public class MyDoorsQueryHandler : IRequestHandler<MyDoorsQuery, List<DoorDto>>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IMapper _mapper;

        public MyDoorsQueryHandler(IDoorRepository doorRepository, IMapper mapper)
        {
            _doorRepository = doorRepository;
            _mapper = mapper;
        }

        public async Task<List<DoorDto>> Handle(MyDoorsQuery request, CancellationToken cancellationToken)
        {
            var doors = await _doorRepository.GetDoorsOfUserAsync(request.UserId);
            return _mapper.Map<List<DoorDto>>(doors);
        }
    }
}
