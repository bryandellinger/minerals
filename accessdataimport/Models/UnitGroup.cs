using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UnitGroup
    {
        public long Id { get; set; }
        public IEnumerable<Unit> Units { get; set; }
    }
}
