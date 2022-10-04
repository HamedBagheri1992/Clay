using MediatR;
using System.Collections.Generic;

namespace ClayService.Application.Features.Door.Commands.AssignDoor
{
    public class AssignDoorCommand : IRequest
    {
        public AssignDoorCommand(long userId, List<long> doorIds, bool isAdmin, long currentUserId)
        {
            UserId = userId;
            DoorIds = doorIds;
            IsAdmin = isAdmin;
            CurrentUserId = currentUserId;
        }

        public long UserId { get; set; }
        public List<long> DoorIds { get; set; }

        public bool IsAdmin { get; set; }
        public long CurrentUserId { get; set; }
    }
}
