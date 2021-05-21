using System.Collections.Generic;

namespace Models
{
    public class Role
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public IEnumerable<UserRoleJunction> UserRoles { get; set; }

    }
}
