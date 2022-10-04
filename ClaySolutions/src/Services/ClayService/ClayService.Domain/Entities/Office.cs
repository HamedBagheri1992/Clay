using System;
using System.Collections.Generic;

namespace ClayService.Domain.Entities
{
    public class Office : EntityBase
    {
        public string Title { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }


        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Door> Doors { get; set; } = new List<Door>();
        //public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();
    }
}
