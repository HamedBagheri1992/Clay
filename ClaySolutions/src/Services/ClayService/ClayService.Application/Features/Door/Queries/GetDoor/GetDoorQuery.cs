using MediatR;

namespace ClayService.Application.Features.Door.Queries.GetDoor
{
    public class GetDoorQuery : IRequest<DoorDto>
    {
        public long DoorId { get; set; }
    }
}
