using System;
using System.Collections.Generic;

namespace Models
{
    public class UnitGroup
    {
        public long Id { get; set; }
        public IEnumerable<Unit> Units { get; set; }

    }
}
