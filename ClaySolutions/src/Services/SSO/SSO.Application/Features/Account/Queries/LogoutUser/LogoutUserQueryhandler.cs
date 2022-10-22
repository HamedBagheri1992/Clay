using MediatR;
using SSO.Application.Contracts.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Account.Queries.LogoutUser
{
    public class LogoutUserQueryhandler : IRequestHandler<LogoutUserQuery>
    {
        private readonly IAuthenticationService _authenticationService;

        public LogoutUserQueryhandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Unit> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.LogoutAsync(request);
            return Unit.Value;
        }
    }
}
