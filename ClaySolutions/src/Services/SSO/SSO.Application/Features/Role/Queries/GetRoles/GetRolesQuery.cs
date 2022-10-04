using MediatR;
using SSO.Application.Features.Role.Queries.GetRole;
using System.Collections.Generic;

namespace SSO.Application.Features.Role.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<List<RoleDto>>
    {
        public string Title { get; set; }
    }
}
