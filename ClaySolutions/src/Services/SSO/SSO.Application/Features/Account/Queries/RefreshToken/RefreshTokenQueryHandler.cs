using MediatR;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.Account.Queries.Authenticate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Account.Queries.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, AuthenticateDto>
    {
        private readonly IAccountRepository _accountRepository;

        public RefreshTokenQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AuthenticateDto> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            return await _accountRepository.RefreshTokenAsync(request);
        }
    }
}
