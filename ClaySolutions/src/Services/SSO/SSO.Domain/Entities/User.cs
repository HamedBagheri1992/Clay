using System;
using System.Collections.Generic;

namespace SSO.Domain.Entities
{
    public class User : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }

        public virtual IList<Role> Roles { get; set; } = new List<Role>();
        public virtual IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
