using MediatR;
using SSO.Application.Features.User.Queries.GetUser;

namespace SSO.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
