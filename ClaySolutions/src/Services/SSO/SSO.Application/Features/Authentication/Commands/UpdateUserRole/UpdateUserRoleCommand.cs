using MediatR;
using System.Collections.Generic;

namespace SSO.Application.Features.Authentication.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommand : IRequest
    {
        public long UserId { get; set; }
        public List<long> RoleIds { get; set; }
    }
}
