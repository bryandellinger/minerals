using System;
using System.Collections.Generic;

namespace Models
{
    public class Surety : AuditedEntity
    {
        public long Id { get; set; }
        public long SuretyTypeId { get; set; }
        public SuretyType SuretyType { get; set; }
        public long? BondCategoryId { get; set; }
        public BondCategory BondCategory { get; set; }
        public long LesseeId { get; set; }
        public Lessee Lessee { get; set; }
        public long? ContractId { get; set; }
        public Contract Contract { get; set; }
        public double? InitialSuretyValue { get; set; }
        public double? CurrentSuretyValue { get; set; }
        public double? ReleasedSuretyValue { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public string Insurer { get; set; }
        public bool ClaimedInd { get; set; }
        public string SuretyNum { get; set; }
        public string SuretyNotes { get; set; }
        public string SuretyStatus { get; set; }
        public IEnumerable<SuretyWell> SuretyWells { get; set; }
        public IEnumerable<SuretyRider> SuretyRiders { get; set; }

    }
}
