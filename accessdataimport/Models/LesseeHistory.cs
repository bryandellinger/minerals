using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class LesseeHistory
    {
        public long Id { get; set; }
        public string LesseeName { get; set; }
        public string CreatedByFirstName { get; set; }
        public string CreatedByLastName { get; set; }
        public long LesseeId { get; set; }
        public Lessee Lessee { get; set; }
    }
}
