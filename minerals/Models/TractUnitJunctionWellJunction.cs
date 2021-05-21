using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class TractUnitJunctionWellJunction
    {
        public long Id { get; set; }
        public long TractUnitJunctionId { get; set; }
        public TractUnitJunction TractUnitJunction { get; set; }
        public long WellId { get; set; }
        public Well Well { get; set; }
    }
}
