using MediatR;

namespace SSO.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
    }
}
