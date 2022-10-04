using MediatR;

namespace SSO.Application.Features.User.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public long Id { get; set; }
    }
}
