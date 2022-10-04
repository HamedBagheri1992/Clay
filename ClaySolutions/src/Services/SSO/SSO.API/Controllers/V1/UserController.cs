using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SSO.Application.Features.User.Commands.CreateUser;
using SSO.Application.Features.User.Commands.DeleteUser;
using SSO.Application.Features.User.Commands.UpdateUser;
using SSO.Application.Features.User.Queries.GetUser;
using SSO.Application.Features.User.Queries.GetUsers;
using System.Threading.Tasks;

namespace SSO.API.Controllers.V1
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ApiControllerBase
    {
        [HttpGet("{Id}")]
        public async Task<ActionResult> Get([FromRoute] GetUserQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        public async Task<ActionResult> Get([FromQuery] GetUsersQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var userDto = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { userId = userDto.Id }, userDto);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        public async Task<IActionResult> Update([FromRoute] long Id, [FromBody] UpdateUserCommand command)
        {
            if (Id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }
    }
}
