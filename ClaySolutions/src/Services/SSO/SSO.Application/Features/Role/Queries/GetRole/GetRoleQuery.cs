using MediatR;

namespace SSO.Application.Features.Role.Queries.GetRole
{
    public class GetRoleQuery : IRequest<RoleDto>
    {
        public long Id { get; set; }
    }
}
