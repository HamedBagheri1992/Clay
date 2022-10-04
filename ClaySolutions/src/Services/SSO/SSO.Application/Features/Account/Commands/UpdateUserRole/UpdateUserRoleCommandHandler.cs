using MediatR;
using SSO.Application.Contracts.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Account.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public UpdateUserRoleCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _accountRepository.UpdateUserRoleAsync(request);
            return Unit.Value;
        }
    }
}
