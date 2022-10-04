using MediatR;

namespace ClayService.Application.Features.Door.Commands.UpdateDoor
{
    public class UpdateDoorCommand : IRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long OfficeId { get; set; }
        public bool IsActive { get; set; }
    }
}
