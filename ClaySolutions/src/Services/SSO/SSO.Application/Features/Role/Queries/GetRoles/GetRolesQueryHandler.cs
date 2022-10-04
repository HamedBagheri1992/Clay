using AutoMapper;
using MediatR;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.Role.Queries.GetRole;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Application.Features.Role.Queries.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAsync(request);
            return _mapper.Map<List<RoleDto>>(roles);
        }
    }
}
