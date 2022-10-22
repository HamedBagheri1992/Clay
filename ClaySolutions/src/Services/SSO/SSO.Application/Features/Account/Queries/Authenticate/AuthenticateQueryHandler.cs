using MediatR;
using SSO.Application.Contracts.Infrastructure;
using SSO.Application.Contracts.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Account.Queries.Authenticate
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, AuthenticateDto>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AuthenticateDto> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            return await _authenticationService.AuthenticateAsync(request);
        }
    }
}
