using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class WellboreShare
    {
        public long Id { get; set; }
        public double LengthInUnit { get; set; }
        public long WellId { get; set; }
        public Well Well { get; set; }
        public long UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
