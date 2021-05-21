using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class DigitalWellLogTestTypeWellJunction
    {
        public long Id { get; set; }
        public long WellLogTestTypeId { get; set; }
        public WellLogTestType WellLogTestType { get; set; }

        public long WellId { get; set; }
        public Well Well { get; set; }

    }
}
