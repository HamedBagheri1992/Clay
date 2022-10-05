using ClayService.Application.Features.Tag.Commands.AssignTag;
using ClayService.Application.Features.Tag.Commands.CreateTag;
using ClayService.Application.Features.Tag.Queries.GetTag;
using ClayService.Application.Features.Tag.Queries.GetTags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using System.Threading.Tasks;

namespace ClayService.API.Controllers.V1
{
    [ApiController]
    [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Manager}")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TagController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<TagDto>>> Get([FromQuery] GetTagsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<TagDto>> Get([FromRoute] GetTagQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


        [Authorize(Roles = SystemRoleDefinition.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagCommand command)
        {
            var tagDto = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { tagId = tagDto.Id }, tagDto);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AssignTagToUser([FromBody] AssignTagCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
