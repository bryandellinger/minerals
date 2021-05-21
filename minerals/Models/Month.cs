using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Month
    {
        public long Id { get; set; }
        public int MonthNum { get; set; }
        public string MonthName { get; set; }

        public IEnumerable<StorageWellPaymentMonthJunction> StorageWellPaymentMonthJunctions { get; set; }
        public IEnumerable<StorageBaseRentalPaymentMonthJunction> StorageBaseRentalPaymentMonthJunctions { get; set; }
        public IEnumerable<StorageRentalPaymentMonthJunction> StorageRentalPaymentMonthJunctions { get; set; }
        public IEnumerable<ContractRentalPaymentMonthJunction> ContractRentalPaymentMonthJunctions { get; set; }
    }
}
