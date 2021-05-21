using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public  class SuretyWell
    {
        public long Id { get; set; }

        public double? SuretyWellValue { get; set; }

        public long WellId { get; set; }
        public Well Well { get; set; }

        public long? SuretyId { get; set; }
        public Surety Surety { get; set; }
    }
}
