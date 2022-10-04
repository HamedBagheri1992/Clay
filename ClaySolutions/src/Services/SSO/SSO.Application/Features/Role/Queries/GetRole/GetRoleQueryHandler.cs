using AutoMapper;
using MediatR;
using SSO.Application.Contracts.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Role.Queries.GetRole
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetRoleQueryHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<RoleDto> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetAsync(request);
            return _mapper.Map<RoleDto>(role);
        }
    }
}
