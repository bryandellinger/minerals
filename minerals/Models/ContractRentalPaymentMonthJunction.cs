using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ContractRentalPaymentMonthJunction
    {
        public long Id { get; set; }
        public long ContractId { get; set; }
        public Contract Contract { get; set; }

        public long MonthId { get; set; }
        public Month Month { get; set; }
    }
}
