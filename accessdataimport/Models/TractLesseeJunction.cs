using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class TractLesseeJunction
    {
        public long Id { get; set; }

        public long LesseeId { get; set; }
        public Lessee Lessee { get; set; }

        public long TractId { get; set; }
        public Tract Tract { get; set; }
    }
}
