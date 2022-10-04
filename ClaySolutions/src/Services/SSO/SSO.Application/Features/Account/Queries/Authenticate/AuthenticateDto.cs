using System;

namespace SSO.Application.Features.Account.Queries.Authenticate
{
    public class AuthenticateDto
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
