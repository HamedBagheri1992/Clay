using ClayService.Application.Features.EventHistory.Queries.GetEventHistories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using System.Threading.Tasks;

namespace ClayService.API.Controllers.V1
{
    [ApiController]   
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = $"{SystemRoleDefinition.Admin},{SystemRoleDefinition.Reporter}")]
    public class EventHistoryController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<EventHistoryDto>>> Get([FromQuery] GetEventHistoriesQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
