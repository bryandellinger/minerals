using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Unit : AuditedEntity
    {
        public long Id { get; set; }
        public string UnitName { get; set; }
        public string AmendmentName { get; set; }
        public double? DPUAcres { get; set; }
        public double? GISAcres { get; set; }
        public string AlternateId { get; set; }
        public DateTime? DPUAcresEffectiveDate { get; set; }
        public bool IsActiveInd { get; set; }
        public IEnumerable<TractUnitJunction> TractUnitJunctions { get; set; }
        public long UnitGroupId { get; set; }
        public UnitGroup UnitGroup { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}
