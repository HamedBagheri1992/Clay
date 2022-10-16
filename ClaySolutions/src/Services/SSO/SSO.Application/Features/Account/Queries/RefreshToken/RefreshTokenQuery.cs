using MediatR;
using SSO.Application.Features.Account.Queries.Authenticate;

namespace SSO.Application.Features.Account.Queries.RefreshToken
{
    public class RefreshTokenQuery : IRequest<AuthenticateDto>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
