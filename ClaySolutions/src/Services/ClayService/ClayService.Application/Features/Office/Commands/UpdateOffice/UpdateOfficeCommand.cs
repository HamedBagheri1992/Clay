using MediatR;

namespace ClayService.Application.Features.Office.Commands.UpdateOffice
{
    public class UpdateOfficeCommand : IRequest
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }
}
