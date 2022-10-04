using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.User.Commands.UserAddOrUpdate
{
    public class UserAddOrUpdateCommandHandler : IRequestHandler<UserAddOrUpdateCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserAddOrUpdateCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UserAddOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Domain.Entities.User>(request);
            return await _userRepository.AddOrUpdateAsync(user);
        }
    }
}
