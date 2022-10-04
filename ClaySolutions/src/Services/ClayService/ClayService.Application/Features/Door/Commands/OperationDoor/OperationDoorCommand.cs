using MediatR;

namespace ClayService.Application.Features.Door.Commands.OperationDoor
{
    public class OperationDoorCommand : IRequest
    {
        public long DoorId { get; set; }
        public long UserId { get; set; }

        public OperationDoorCommand(long doorId, long userId)
        {
            DoorId = doorId;
            UserId = userId;
        }        
    }
}
