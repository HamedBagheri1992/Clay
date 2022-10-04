using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClayService.Domain.Entities
{
    public class PhysicalTag : EntityBase
    {
        public Guid TagCode { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}
