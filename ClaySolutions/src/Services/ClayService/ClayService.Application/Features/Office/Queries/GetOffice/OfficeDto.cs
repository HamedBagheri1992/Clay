using System;

namespace ClayService.Application.Features.Office.Queries.GetOffice
{
    public class OfficeDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
