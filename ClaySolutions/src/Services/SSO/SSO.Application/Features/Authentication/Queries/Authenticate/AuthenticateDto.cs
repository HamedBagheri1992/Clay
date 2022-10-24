using System;

namespace SSO.Application.Features.Authentication.Queries.Authenticate
{
    public class AuthenticateDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
