using System.Collections.Generic;

namespace ClayService.Application.Features.Door.Commands.AssignDoor
{
    public class AssignDoorToUserDto
    {
        public long UserId { get; set; }
        public List<long> DoorIds { get; set; }
    }
}
