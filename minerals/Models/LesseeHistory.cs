using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class LesseeHistory : AuditedEntity
    {
        public long Id { get; set; }
        public string LesseeName { get; set; }
        public string CreatedByFirstName { get; set; }
        public string CreatedByLastName { get; set; }
        public long LesseeId { get; set; }
        public Lessee Lessee { get; set; }
    }
}
