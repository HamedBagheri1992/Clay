using MediatR;

namespace ClayService.Application.Features.Office.Queries.GetOffice
{
    public class GetOfficeQuery : IRequest<OfficeDto>
    {
        public long Id { get; set; }       
    }
}
