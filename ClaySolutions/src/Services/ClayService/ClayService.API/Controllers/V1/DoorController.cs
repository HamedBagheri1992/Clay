using ClayService.Application.Features.Door.Commands.AssignDoor;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Commands.OperationDoor;
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
    //[Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DoorController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<DoorDto>>> Get([FromQuery] GetDoorsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoorCommand command)
        {
            var doorDto = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { doorId = doorDto.Id }, doorDto);
        }


        [HttpPost("[action]/{doorId}")]
        public async Task<IActionResult> Operation([FromRoute] long doorId)
        {
            var command = new OperationDoorCommand(doorId, CurrentUser.Id);
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("[action]")]
        [Authorize("Admin,Manager")]
        public async Task<IActionResult> AssignDoorToUser([FromBody] AssignDoorCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
