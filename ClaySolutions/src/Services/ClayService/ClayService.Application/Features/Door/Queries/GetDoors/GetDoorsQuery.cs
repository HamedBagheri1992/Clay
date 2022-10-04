using ClayService.Application.Features.Door.Queries.GetDoor;
using MediatR;
using SharedKernel.Common;

namespace ClayService.Application.Features.Door.Queries.GetDoors
{
    public class GetDoorsQuery : PaginationQuery, IRequest<PaginatedList<DoorDto>>
    {
        public string Name { get; set; }
        public long? OfficeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
