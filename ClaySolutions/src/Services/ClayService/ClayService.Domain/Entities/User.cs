using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClayService.Domain.Entities
{
    public class User : EntityBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public long? PhysicalTagId { get; set; }

        [ForeignKey("PhysicalTagId")]
        public virtual PhysicalTag PhysicalTag { get; set; }

        public virtual ICollection<Office> Offices { get; set; } = new List<Office>();
        public virtual ICollection<Door> Doors { get; set; } = new List<Door>();
        //public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();
    }
}
