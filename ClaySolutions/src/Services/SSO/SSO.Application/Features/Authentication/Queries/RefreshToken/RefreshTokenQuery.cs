using MediatR;
using SSO.Application.Features.Authentication.Queries.Authenticate;

namespace SSO.Application.Features.Authentication.Queries.RefreshToken
{
    public class RefreshTokenQuery : IRequest<AuthenticateDto>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
