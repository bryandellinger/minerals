using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class Well
    {
        public long Id { get; set; }

        [NotMapped]
        public long PKWellId { get; set; }

        [NotMapped]
        public long? FkUnitId { get; set; }

        public string WellNum { get; set; }

        public string ApiNum { get; set; }

        public string WellCode { get; set; }

        public int? VDepth { get; set; }

        public int? HDepth { get; set; }

        public int? Drainage { get; set; }

        public DateTime? BofAppDate { get; set; }

        public DateTime? PermitDate { get; set; }

        public DateTime? SpudDate{ get; set; }

        public DateTime? PlugDate { get; set; }
    
        public double? Lat { get; set; }

        public double? Long { get; set; }

        public string Notes { get; set; }

        public bool? WellLocation { get; set; }
        public bool WellboreLengthInd { get; set; }

        public decimal? RoyaltyPercentOverride { get; set; }

        public string Horizon { get; set; }

        public string Formation { get; set; }

        public bool? Severed { get; set; }

        public int? Elevation { get; set; }
        public int? GLElevation { get; set; }
        public int? LogStartDepth { get; set; }
        public int? LogEndDepth { get; set; }
        public string LogNotes { get; set; }

        public bool? BelowGroundInd { get; set; }
        public bool? PrivatePropertyInd { get; set; }
        public bool AutoUpdatedAllowedInd { get; set; }

        public string AltIdType { get; set; }
        public string AltId { get; set; }
        public double? AcreageAttributableToWells { get; set; }

        public double? TotalBoreholeLength { get; set; }
        public bool TotalBoreholeLengthOverrideInd { get; set; }

        public long? PadId { get; set; }
        public Pad Pad { get; set; }

        public long? ContractId { get; set; }
        public Contract Contract { get; set; }

        public long WellStatusId { get; set; }
        public WellStatus WellStatus { get; set; }

        public long? LesseeId { get; set; }
        public Lessee Lessee { get; set; }

        public long? WellTypeId { get; set; }
        public WellType WellType { get; set; }

        public long? TownshipId { get; set; }
        public Township Township { get; set; }

        public long? SuretyId { get; set; }
        public Surety Surety { get; set; }


        public long? DeepestFormationId { get; set; }
        [ForeignKey("DeepestFormationId")]
        public Formation DeepestFormation { get; set; }
        public long? ProducingFormationId { get; set; }
        [ForeignKey("ProducingFormationId")]
        public Formation ProducingFormation { get; set; }

        public IEnumerable<WellOperation> WellOperations { get; set; }
        public IEnumerable<WellTractInformation> WellTractInformations { get; set; }
        public IEnumerable<TractUnitJunctionWellJunction> TractUnitJunctionWellJunctions { get; set; }
        public IEnumerable<WellboreShare> WellboreShares { get; set; }
    }
}
