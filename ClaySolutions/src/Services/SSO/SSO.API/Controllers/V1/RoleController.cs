using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SSO.Application.Features.Role.Queries.GetRole;
using SSO.Application.Features.Role.Queries.GetRoles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSO.API.Controllers.V1
{
    [ApiController]
    [Authorize(Roles = SystemRoleDefinition.Admin)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoleController : ApiControllerBase
    {
        [HttpGet("{Id}")]
        public async Task<ActionResult<RoleDto>> Get([FromRoute] GetRoleQuery query)
        {
            var roleDto = await Mediator.Send(query);
            return Ok(roleDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> Get([FromQuery] GetRolesQuery query)
        {
            var rolesDto = await Mediator.Send(query);
            return Ok(rolesDto);
        }
    }
}
