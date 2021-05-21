using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class District
    {
        public long Id { get; set; }

        public long DistrictId { get; set; }

        public string Name { get; set; }

        public IEnumerable<DistrictContractJunction> DistrictContacts { get; set; }
        public IEnumerable<DistrictTractJunction> DistrictTracts { get; set; }
    }
}
