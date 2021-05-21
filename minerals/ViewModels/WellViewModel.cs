using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class WellViewModel
    {
        public long WellId { get; set; }
        public string WellNum { get; set; }
        public string ApiNum { get; set; }
        public long TractId { get; set; }
        public string TractNum { get; set; }
        public long PadId { get; set; }
        public string PadName { get; set; }
        public long WellStatusId { get; set; }
        public long? LesseeId { get; set; }
        public long? WellTypeId { get; set; }
        public int? Elevation { get; set; }
        public int? GLElevation { get; set; }
        public int? LogStartDepth { get; set; }
        public int? LogEndDepth { get; set; }
        public string LogNotes { get; set; }
        public bool? BelowGroundInd { get; set; }
        public bool? PrivatePropertyInd { get; set; }
        public bool WellboreLengthInd { get; set; }
        public string AltIdType { get; set; }
        public string AltId { get; set; }
        public long? Contractid { get; set; }
        public long? TownshipId { get; set; }
        public int? VDepth { get; set; }

        public int? HDepth { get; set; }
        public DateTime? SpudDate { get; set; }
        public DateTime? InitialProductionDate { get; set; }
        public DateTime? BofAppDate { get; set; }
        public DateTime? PlugDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? ShutInDate { get; set; }
        public double? Lat { get; set; }
        public long? DeepestFormationId { get; set; }
        public long? ProducingFormationId { get; set; }
        public long? ContractEventDetailReasonsForChangeId { get; set; }
        public bool AutoUpdatedAllowedInd { get; set; }
        public double? AcreageAttributableToWells { get; set; }

#pragma warning disable CA1720 // Identifier contains type name
        public double? Long { get; set; }
        public double? TotalBoreholeLength { get; set; }
        public bool TotalBoreholeLengthOverrideInd { get; set; }
#pragma warning restore CA1720 // Identifier contains type name
        public IEnumerable<WellTractInformationViewModel> WellTractInfos { get; set; }
        public IEnumerable<long> DigitalLogs { get; set; }
        public IEnumerable<long> DigitalImageLogs { get; set; }
        public IEnumerable<long> HardCopyLogs { get; set; }
        public IEnumerable<WellboreShare> wellboreShares { get; set; }
    }
}
