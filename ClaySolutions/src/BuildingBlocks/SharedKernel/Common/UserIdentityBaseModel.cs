using System.Collections.Generic;

namespace SharedKernel.Common
{
    public class UserIdentityBaseModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public List<string> Roles { get; set; }
    }
}
