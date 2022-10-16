using MediatR;
using SSO.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Account.Queries.LogoutUser
{
    public class LogoutUserQueryhandler : IRequestHandler<LogoutUserQuery>
    {
        private readonly IAccountRepository _accountRepository;

        public LogoutUserQueryhandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
        {
            await _accountRepository.LogoutAsync(request);
            return Unit.Value;
        }
    }
}
