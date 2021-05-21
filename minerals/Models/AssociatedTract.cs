using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class AssociatedTract
    {
        public long Id { get; set; }
        public string TractNum { get; set; }
        public long ContractId { get; set; }
    }
}
