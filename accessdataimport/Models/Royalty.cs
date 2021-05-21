using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Royalty 
    {
        public long Id { get; set; }

        [NotMapped]
        public long? PKRoyaltyId { get; set; }

        [NotMapped]
        public long? PKLiqRoyId { get; set; }

        [NotMapped]
        public long? FKWellOpsId { get; set; }

        [NotMapped]
        public long? FKCheckId { get; set; }

        public int? PostMonth { get; set; }

        public int? PostYear { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal?  GasRoyalty { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? OilRoyalty { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? GasProd { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? OilProd { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? Deduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? TransDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? CompressDeduction { get; set; }

        public bool? Flaring { get; set; }

        public DateTime? RecvDate { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? TransmittalDate { get; set; }

        public string RoyaltyNotes { get; set; }

        public bool? FirstTimePayment { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? SalesPrice { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? TrOpsShare { get; set; }

    
        public decimal? RentPayment { get; set; }

    
        public decimal? StoragePayment { get; set; }

    
        public decimal? OtherPayment { get; set; }


        [Column(TypeName = "decimal(23, 15)")]
        public decimal? NRI { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? UAFuelDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? QGatheringDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? QCompressionDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? CompressionDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? MarketingDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? EightTTransDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? CompDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? FeulDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? GGatheringDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? NineTTransDeduction { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? AOMiscDeduction { get; set; }

        public long? RevenueReceivedId { get; set; }

        public RevenueReceived RevenueReceived { get; set; }

        public long? WellOperationId { get; set; }

        public WellOperation WellOperation { get; set; }
        public long? WellTractInformationId { get; set; }
        public WellTractInformation WellTractInformation { get; set; }
        public string CheckNum { get; set; }
        public DateTime? CheckDate { get; set; }
        public long? CheckId { get; set; }
        public Check Check { get; set; }

        public long PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

        public long? ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        public IEnumerable<RoyaltyAdjustment> RoyaltyAdjustments { get; set; }
        public decimal? LiqVolume { get; set; }
        [Column(TypeName = "decimal(23, 15)")]
        public decimal? LiqPayment { get; set; }

        public string LiqMeasurement { get; set; }

    }
}
