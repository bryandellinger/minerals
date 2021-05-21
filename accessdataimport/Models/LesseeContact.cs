using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class LesseeContact
    {
        public long Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }

        public long LesseeId { get; set; }
        public Lessee Lessee { get; set; }
    }
}
