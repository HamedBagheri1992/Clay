using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using SSO.Application.Contracts.Infrastructure;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.User.Queries.GetUser;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IPublishEndpoint publishEndpoint, IUserRepository userRepository, IEncryptionService encryptionService, IDateTimeService dateTimeService, IMapper mapper)
        {
            _publishEndpoint = publishEndpoint;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _dateTimeService = dateTimeService;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.Entities.User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Password = _encryptionService.HashPassword(request.Password),
                IsActive = request.IsActive,
                CreatedDate = _dateTimeService.Now
            };

            var result = await _userRepository.CreateAsync(user);

            var userCheckoutEvent = new UserCheckoutEvent { UserId = result.Id, UserName = result.UserName, DisplayName = $"{result.FirstName} {result.LastName}" };
            await _publishEndpoint.Publish(userCheckoutEvent);

            return _mapper.Map<UserDto>(user);
        }
    }
}
