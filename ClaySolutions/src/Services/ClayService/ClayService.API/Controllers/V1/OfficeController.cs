using ClayService.Application.Features.Office.Commands.CreateOffice;
using ClayService.Application.Features.Office.Queries.GetOffice;
using ClayService.Application.Features.Office.Queries.GetOffices;
using ClayService.Application.Features.Office.Queries.MyOffices;
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
    public class OfficeController : ApiControllerBase
    {
        [HttpGet]
        [Authorize($"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Reporter}")]
        public async Task<ActionResult<PaginatedList<OfficeDto>>> Get([FromQuery] GetOfficesQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<OfficeDto>> Get([FromRoute] GetOfficeQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<OfficeDto>>> MyOffices()
        {
            var query = new MyOfficesQuery(CurrentUser.Id);
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOfficeCommand command)
        {
            var officeDto = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { officeId = officeDto.Id }, officeDto);
        }
    }
}
