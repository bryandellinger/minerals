using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public  class StorageMonthLookup
    {
        public string LeaseNum { get; set; }
        public int[] BaseRentalMonths { get; set; }
        public int[] RentalMonths { get; set; }
        public int[] WellMonths { get; set; }
    }
}
