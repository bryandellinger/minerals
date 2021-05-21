using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class RoyaltyAdjustment : AuditedEntity
    {
        public long Id { get; set; }

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? GasRoyalty { get; set; }

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

        [Column(TypeName = "decimal(23, 15)")]
        public decimal? SalesPrice { get; set; }

        public long? RoyaltyId { get; set; }
        public Royalty Royalty { get; set; }
        public decimal? NRI { get; set; }
        public DateTime? EntryDate { get; set; }

        public long? CheckId { get; set; }
        public Check Check { get; set; }
        public decimal? LiqVolume { get; set; }
        [Column(TypeName = "decimal(23, 15)")]
        public decimal? LiqPayment { get; set; }
    }
}
