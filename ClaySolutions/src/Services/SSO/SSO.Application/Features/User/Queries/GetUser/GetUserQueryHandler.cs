using AutoMapper;
using MediatR;
using SharedKernel.Exceptions;
using SSO.Application.Contracts.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.User.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.Id);
            if (user == null)
                throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

            return _mapper.Map<UserDto>(user);
        }
    }
}
