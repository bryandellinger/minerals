using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class PluggingSuretyDetail
    {
        public long Id { get; set; }
        public string MeasurementType { get; set; }
        public int? FromDepth { get; set; }
        public int? ToDepth { get; set; }
        public decimal? MinimumBondAmount { get; set; }
        public long PaymentRequirementId { get; set; }
    }
}
