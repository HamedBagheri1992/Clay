using MediatR;

namespace ClayService.Application.Features.Office.Queries.GetOffice
{
    public class GetOfficeQuery : IRequest<OfficeDto>
    {
        public long OfficeId { get; set; }

        public GetOfficeQuery(long officeId)
        {
            OfficeId = officeId;
        }
    }
}
