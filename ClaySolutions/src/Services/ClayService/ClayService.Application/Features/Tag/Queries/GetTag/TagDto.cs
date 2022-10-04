using System;

namespace ClayService.Application.Features.Tag.Queries.GetTag
{
    public class TagDto
    {
        public long Id { get; set; }
        public string TagCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
