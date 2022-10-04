using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClayService.Domain.Entities
{
    public class Door : EntityBase
    {
        public string Name { get; set; }
        public long OfficeId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }

        [ForeignKey("OfficeId")]
        public virtual Office Office { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        //public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();
    }
}
