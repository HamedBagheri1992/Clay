using System;

namespace ClayService.Domain.Entities
{
    public class PhysicalTag : EntityBase
    {
        public string TagCode { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}
