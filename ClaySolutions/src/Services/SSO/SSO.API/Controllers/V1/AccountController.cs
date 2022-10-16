using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SSO.Application.Features.Account.Commands.ChangePassword;
using SSO.Application.Features.Account.Commands.UpdateUserRole;
using SSO.Application.Features.Account.Queries.Authenticate;
using SSO.Application.Features.Account.Queries.LogoutUser;
using SSO.Application.Features.Account.Queries.RefreshToken;
using System.Threading.Tasks;

namespace SSO.API.Controllers.V1
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<AuthenticateDto>> Login([FromBody] AuthenticateQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<ActionResult<AuthenticateDto>> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            if (command.UserId != CurrentUser.Id)
                return BadRequest();

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("[action]")]
        [Authorize(Roles = SystemRoleDefinition.Admin)]
        public async Task<ActionResult<AuthenticateDto>> UpdateUserRole([FromBody] UpdateUserRoleCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout([FromBody] LogoutUserQuery query)
        {
            if (CurrentUser.Id != query.UserId)
                return BadRequest();

            await Mediator.Send(query);
            return NoContent();
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
