using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class StorageWellPaymentMonthJunction
    {
        public long Id { get; set; }
        public long StorageId { get; set; }
        public Storage Storage { get; set; }

        public long MonthId { get; set; }
        public Month Month { get; set; }
    }
}
