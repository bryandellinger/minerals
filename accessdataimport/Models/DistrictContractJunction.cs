using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class DistrictContractJunction
    {
        public long Id { get; set; }
        public long ContractId { get; set; }
        public Contract Contract { get; set; }

        public long DistrictId { get; set; }
        public District District { get; set; }

    }
}
 