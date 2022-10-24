using MediatR;
using SSO.Application.Contracts.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Authentication.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
    {
        private readonly IAuthenticationService _authenticationService;

        public UpdateUserRoleCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _authenticationService.UpdateUserRoleAsync(request);
            return Unit.Value;
        }
    }
}
