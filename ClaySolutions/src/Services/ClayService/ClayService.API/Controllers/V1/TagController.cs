using ClayService.Application.Features.Office.Queries.GetOffice;
using ClayService.Application.Features.Tag.Commands.CreateTag;
using ClayService.Application.Features.Tag.Queries.GetTag;
using ClayService.Application.Features.Tag.Queries.GetTags;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using System.Threading.Tasks;

namespace ClayService.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<TagDto>>> Get([FromQuery] GetTagsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<TagDto>> Get([FromQuery] GetTagQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTagCommand command)
        {
            var tagDto = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { tagId = tagDto.Id }, tagDto);
        }
    }
}
