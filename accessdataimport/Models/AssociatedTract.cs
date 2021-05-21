using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class AssociatedTract
    {
        public long Id { get; set; }
        public string TractNum { get; set; }
        public long ContractId { get; set; }
    }
}
