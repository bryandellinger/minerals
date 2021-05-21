using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class AssociatedContract
    {
        public long Id { get; set; }
        public string AssociatedContractName { get; set; }
        public long ContractId { get; set; }
    }
}
