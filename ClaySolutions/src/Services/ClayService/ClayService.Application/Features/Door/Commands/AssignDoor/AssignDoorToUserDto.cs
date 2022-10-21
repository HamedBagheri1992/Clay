using System.Collections.Generic;

namespace ClayService.Application.Features.Door.Commands.AssignDoor
{
    public class AssignDoorToUserDto
    {
        public long UserId { get; set; }
        public long DoorId { get; set; }
    }
}
