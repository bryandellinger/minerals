using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class RowContract
    {
        public long Id { get; set; }
        public string RowContractName { get; set; }
        public long ContractId { get; set; }
    }
}
