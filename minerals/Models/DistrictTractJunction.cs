using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class DistrictTractJunction
    {
        public long Id { get; set; }
        public long TractId { get; set; }
        public Tract Tract { get; set; }

        public long DistrictId { get; set; }
        public District District { get; set; }
    }
}
