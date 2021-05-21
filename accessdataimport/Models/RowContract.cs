using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class RowContract
    {
        public long Id { get; set; }
        public string RowContractName { get; set; }
        public long ContractId { get; set; }
    }
}
