using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class WellTractInformation
    {
        public long Id { get; set; }
        public long? TractId { get; set; }
        public Tract Tract { get; set; }
        public long? PadId { get; set; }
        public Pad Pad { get; set; }
        public long? ContractId { get; set; }
        public Contract Contract { get; set; }
        public long? LesseeId { get; set; }
        public Lessee Lessee { get; set; }
        public string LesseeName { get; set; }
        public decimal? PercentOwnership { get; set; }
        public decimal? RoyaltyPercent { get; set; }
        public long? ContractEventDetailReasonForChangeId { get; set; }
        public bool ActiveInd { get; set; }
        public DateTime? ChangeDate { get; set; }
        public long WellId { get; set; }
        public Well Well { get; set; }
        [NotMapped]
        public long? WellOperationId { get; set; }
        [NotMapped]
        public long? PKWellOpsId { get; set; }
    }
}
