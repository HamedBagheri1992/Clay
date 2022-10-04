using MediatR;

namespace SSO.Application.Features.Account.Queries.Authenticate
{
    public class AuthenticateQuery : IRequest<AuthenticateDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
