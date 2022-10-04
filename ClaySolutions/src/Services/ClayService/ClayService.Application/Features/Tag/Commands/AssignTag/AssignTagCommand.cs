using MediatR;

namespace ClayService.Application.Features.Tag.Commands.AssignTag
{
    public class AssignTagCommand : IRequest
    {
        public long UserId { get; set; }
        public long TagId { get; set; }
        public bool RemoveRequest { get; set; } = false;
    }
}
