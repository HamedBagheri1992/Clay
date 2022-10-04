using MediatR;

namespace ClayService.Application.Features.Office.Commands.DeleteOffice
{
    public class DeleteOfficeCommand : IRequest
    {
        public long Id { get; set; }
    }
}
