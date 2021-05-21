using System;
using System.Collections.Generic;

namespace Models
{
    public class User
    {
        public long Id { get; set; }
        public string EmployeeNum { get; set; }
        public string DomainName { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string EmailAddress { get; set; }
        public string WorkPhone { get; set; }
        public string Company { get; set; }
        public string WorkAddr { get; set; }
        public string JobTitle { get; set; }
        public bool ActiveEmployee { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public IEnumerable<UserRoleJunction> UserRoles { get; set; }
    }
}
