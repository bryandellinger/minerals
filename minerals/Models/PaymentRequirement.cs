using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class PaymentRequirement
    {
        public long Id { get; set; }
        public int? CheckSubmissionPeriod { get; set; }
        public decimal? FirstYearRentalBonusAmount { get; set; }
        public decimal? TotalBonusAmount { get; set; }
        public decimal? LeaseExtensionBonus { get; set; }
        public DateTime? RentalShutInDueDate { get; set; }
        public decimal? SecondThroughFourthYearDelayRental { get; set; }
        public decimal? SecondThroughFourthYearShutInRate { get; set; }
        public string SecondThroughFourthYearShutInRateInterval { get; set; }
        public decimal? FifthYearOnwardDelayRental { get; set; }
        public decimal? FifthYearOnwardShutInRate { get; set; }
        public string ShutInPaymentInterval { get; set; }
        public decimal? StorageFee { get; set; }
        public int? AllowableVariancePerAuditFieldPercent { get; set; }
        public decimal?  ProducerPriceIndex { get; set; }
        public bool? FiveYearInflationIntervalPeriodInd { get; set; }
        public int? TestOfWellEconomy { get; set; }
        public decimal? PerformanceSurety { get; set; }
        public decimal? AgreementFee { get; set; }

        public IEnumerable<PluggingSuretyDetail> PluggingSuretyDetails { get; set; }
        public IEnumerable<AdditionalBonus> AdditionalBonuses { get; set; }

    }
}
