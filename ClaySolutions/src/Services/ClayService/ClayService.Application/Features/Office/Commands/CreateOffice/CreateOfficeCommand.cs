using ClayService.Application.Features.Office.Queries.GetOffice;
using MediatR;

namespace ClayService.Application.Features.Office.Commands.CreateOffice
{
    public class CreateOfficeCommand : IRequest<OfficeDto>
    {
        public string Title { get; set; }
    }
}
