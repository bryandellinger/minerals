using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class RoyaltyViewModel : Royalty
    {
        public bool AdjustmentInd { get; set; }
        public bool SaveNewPaymentAsAdjustmentInd { get; set; }
        public DateTime? AdjustmentEntryDate { get; set; }
        public long LesseeId { get; set; }
        public long WellId { get; set; }

    }
}
