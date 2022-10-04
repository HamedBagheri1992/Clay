using System;

namespace ClayService.Application.Features.Tag.Queries.GetTag
{
    public class TagDto
    {
        public long Id { get; set; }
        public Guid TagCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
