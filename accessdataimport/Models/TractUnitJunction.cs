using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class TractUnitJunction
    {
        public long Id { get; set; }
        public long TractId { get; set; }
        public Tract Tract { get; set; }
        public long UnitId { get; set; }
        public Unit Unit { get; set; }
        public double? COPAcres { get; set; }
        public IEnumerable<TractUnitJunctionWellJunction> TractUnitJunctionWellJunctions { get; set; }
    }
}
