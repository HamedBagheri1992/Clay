using MediatR;

namespace SSO.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public long Id { get; set; }

        public DeleteUserCommand(long id)
        {
            Id = id;
        }
    }
}
