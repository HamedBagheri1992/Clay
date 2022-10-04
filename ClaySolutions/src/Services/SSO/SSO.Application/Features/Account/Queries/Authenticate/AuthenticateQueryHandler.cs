using MediatR;
using SSO.Application.Contracts.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Account.Queries.Authenticate
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, AuthenticateDto>
    {
        private readonly IAccountRepository _accountRepository;

        public AuthenticateQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AuthenticateDto> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            return await _accountRepository.AuthenticateAsync(request);
        }
    }
}
