using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
