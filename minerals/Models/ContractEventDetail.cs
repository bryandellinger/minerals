using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ContractEventDetail
    {
        public long Id { get; set; }

        public string LesseeName { get; set; }
        public string InterestType { get; set; }
        public string TopVerticalExtentOfOwnership { get; set; }
        public string BottomVerticalExtentOfOwnership { get; set; }
        public string ExcludedFromVerticalExtentOfOwnership { get; set; }
        public string Description { get; set; }
        public string Horizon { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? ShareOfLeasePercentage { get; set; }
        public decimal? Acres { get; set; }
        public decimal? CurrentTotalAcres { get; set; }
        public decimal? PreviousTotalAcres { get; set; }
        public bool ActiveInd { get; set; }
        public long ContractId { get; set; }
        public long? ContractEventDetailReasonForChangeId { get; set; }
        public long? LesseeId { get; set; }
        public bool IndustryLeaseInd { get; set; }
        public decimal? MinimumRoyalty { get; set; }
        public decimal? RoyaltyPercent { get; set; }
        public decimal? MinimumRoyaltySalesPrice { get; set; }

    }
}
