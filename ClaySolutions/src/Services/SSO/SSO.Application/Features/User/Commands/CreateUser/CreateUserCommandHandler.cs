using AutoMapper;
using MediatR;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.User.Queries.GetUser;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.CreateAsync(request);
            return _mapper.Map<UserDto>(user);
        }
    }
}
