using ClayService.Application.Features.Office.Queries.GetOffice;
using MediatR;
using SharedKernel.Common;

namespace ClayService.Application.Features.Office.Queries.GetOffices
{
    public class GetOfficesQuery : PaginationQuery, IRequest<PaginatedList<OfficeDto>>
    {
        public string Title { get; set; }
    }
}
