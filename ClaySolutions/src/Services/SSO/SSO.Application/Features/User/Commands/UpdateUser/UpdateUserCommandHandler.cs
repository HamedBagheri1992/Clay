using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using SharedKernel.Exceptions;
using SSO.Application.Contracts.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IPublishEndpoint publishEndpoint, IUserRepository userRepository, IMapper mapper)
        {
            _publishEndpoint = publishEndpoint;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userToUpdate = await _userRepository.GetAsync(request.Id);
            if (userToUpdate == null)
                throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

            _mapper.Map(request, userToUpdate);

            await _userRepository.UpdateAsync(userToUpdate);

            var userCheckoutEvent = new UserCheckoutEvent { UserId = userToUpdate.Id, UserName = userToUpdate.UserName, DisplayName = $"{userToUpdate.FirstName} {userToUpdate.LastName}" };
            await _publishEndpoint.Publish(userCheckoutEvent);

            return Unit.Value;
        }
    }
}
