using MediatR;
using SSO.Application.Contracts.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateAsync(request);
            return Unit.Value;
        }
    }
}
