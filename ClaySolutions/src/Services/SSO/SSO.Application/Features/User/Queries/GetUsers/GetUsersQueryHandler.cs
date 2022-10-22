using AutoMapper;
using MediatR;
using SharedKernel.Common;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.User.Queries.GetUser;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.User.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var pagedusers = await _userRepository.GetAsync(request.UserName, request.FirstName, request.LastName, request.IsActive, request.PageNumber, request.PageSize);
            return _mapper.Map<PaginatedList<UserDto>>(pagedusers);
        }
    }
}
