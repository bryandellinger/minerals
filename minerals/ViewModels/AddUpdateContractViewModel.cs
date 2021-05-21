using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class AddUpdateContractViewModel
    {
        public long Id { get; set; }
        public string ContractNum { get; set; }
        public string Sequence { get; set; }
        public string TractNum { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? ReversionDate { get; set; }
        public DateTime? LesseeEffectiveDate { get; set; }
        public string TerminationReason { get; set; }
        public decimal? InPoolAcreage { get; set; }
        public decimal? BufferAcreage { get; set; }
        public decimal? NSDAcreage { get; set; }
        public bool NSDAcreageAppliesToAllInd { get; set; }
        public decimal? Acreage { get; set; }
        public decimal? InitialAcreage { get; set; }
        public long? ContractTypeId { get; set; }
        public long? ContractSubTypeId { get; set; }
        public string AltIdInformation { get; set; }
        public long? AltIdCategoryId { get; set; }
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
        public decimal? ProducerPriceIndex { get; set; }
        public bool? FiveYearInflationIntervalPeriodInd { get; set; }
        public int? TestOfWellEconomy { get; set; }
        public decimal? PerformanceSurety { get; set; }
        public decimal? AgreementFee { get; set; }
        public DateTime? DataReceivedDate { get; set; }
        public bool? ContractNumOverride { get; set; }
        public long ReasonForChangeId { get; set; }
        public string ReasonForChangeDescription { get; set; }
        public bool updateWellInfoInd { get; set; }
        public decimal? ContractRentalPayment { get; set; }
       public bool  ContractRentalPaymentOverride { get; set; }
       public long? AdditionalContractInformationId { get; set; }

        public IEnumerable<long> DistrictIds { get; set; }
        public IEnumerable<long> StorageWellPaymentMonthIds { get; set; }
        public IEnumerable<long> StorageBaseRentalPaymentMonthIds { get; set; }
        public IEnumerable<long> StorageRentalPaymentMonthIds { get; set; }
        public IEnumerable<long> ContractRentalPaymentMonthIds { get; set; }
        public IEnumerable<long> TownShipIds { get; set; }
       public IEnumerable<string> RowContracts { get; set; }
        public IEnumerable<decimal> AdditionalBonuses { get;  set; }
       public IEnumerable<PluggingSuretyDetail> PluggingSuretyDetails { get; set; }
       public IEnumerable<string> AssociatedContracts { get; set; }
       public IEnumerable<string> AssociatedTracts { get; set; }
       public IEnumerable<ContractEventDetail> ContractEventDetails { get; set; }
       public Storage Storage { get; set; }

    }
}
