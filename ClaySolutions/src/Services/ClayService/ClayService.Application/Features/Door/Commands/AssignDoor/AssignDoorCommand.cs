using MediatR;

namespace ClayService.Application.Features.Door.Commands.AssignDoor
{
    public class AssignDoorCommand : IRequest
    {
        public AssignDoorCommand(long userId, long doorId, bool isAdmin, long currentUserId)
        {
            UserId = userId;
            DoorId = doorId;
            IsAdmin = isAdmin;
            CurrentUserId = currentUserId;
        }

        public long UserId { get; set; }
        public long DoorId { get; set; }

        public bool IsAdmin { get; set; }
        public long CurrentUserId { get; set; }
    }
}
