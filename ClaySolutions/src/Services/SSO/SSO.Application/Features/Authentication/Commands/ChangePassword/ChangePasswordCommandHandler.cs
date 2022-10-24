using MediatR;
using SSO.Application.Contracts.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Authentication.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IAuthenticationService _authenticationService;

        public ChangePasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _authenticationService.ChangePasswordAsync(request);
            return Unit.Value;
        }
    }
}
