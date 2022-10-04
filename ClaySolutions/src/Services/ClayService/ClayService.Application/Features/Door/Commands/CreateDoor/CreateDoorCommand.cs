using ClayService.Application.Features.Door.Queries.GetDoor;
using MediatR;

namespace ClayService.Application.Features.Door.Commands.CreateDoor
{
    public class CreateDoorCommand : IRequest<DoorDto>
    {
        public string Name { get; set; }
        public long OfficeId { get; set; }
    }
}
