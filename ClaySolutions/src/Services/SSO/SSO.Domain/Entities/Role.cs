using System.Collections.Generic;

namespace SSO.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Title { get; set; }

        public virtual IList<User> Users { get; set; } = new List<User>();
    }
}
