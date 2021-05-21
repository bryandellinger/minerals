using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class SuretyRider
    {
        public long Id { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string SuretyRiderNotes { get; set; }
        public long RiderReasonId { get; set; }
        public RiderReason RiderReason {get; set;}
        public long SuretyId { get; set; }
        public Surety Surety { get; set; }
    }
}
