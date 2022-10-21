using ClayService.Application.Features.Door.Commands.AssignDoor;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Commands.OperationDoor;
using ClayService.Application.Features.Door.Commands.UpdateDoor;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Application.Features.Door.Queries.GetDoors;
using ClayService.Application.Features.Door.Queries.MyDoors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.API.Controllers.V1
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DoorController : ApiControllerBase
    {
        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<DoorDto>>> Get([FromQuery] GetDoorsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        [HttpGet("{DoorId}")]
        public async Task<ActionResult<DoorDto>> Get([FromRoute] GetDoorQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<DoorDto>>> MyDoors()
        {
            var query = new MyDoorsQuery(CurrentUser.Id);
            return Ok(await Mediator.Send(query));
        }

        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoorCommand command)
        {
            var doorDto = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { doorId = doorDto.Id }, doorDto);
        }

        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] long Id, [FromBody] UpdateDoorCommand command)
        {
            if (command.Id != Id)
                return BadRequest();

            await Mediator.Send(command);
            return NoContent();
        }


        [HttpPost("[action]/{doorId}")]
        public async Task<IActionResult> Operation([FromRoute] long doorId)
        {
            var command = new OperationDoorCommand(doorId, CurrentUser.Id);
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("[action]")]
        [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
        public async Task<IActionResult> AssignDoorToUser([FromBody] AssignDoorToUserDto assignDto)
        {
            var currentUser = CurrentUser;
            bool isAdmin = currentUser.Roles.Contains(SystemRoleDefinition.Admin);
            var command = new AssignDoorCommand(assignDto.UserId, assignDto.DoorId, isAdmin, currentUser.Id);
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
