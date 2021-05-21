using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Unit
    {
        public long Id { get; set; }
        [NotMapped]
        public long PKUnitId { get; set; }
        public string UnitName { get; set; }
        public string AmendmentName { get; set; }
        public string AlternateId { get; set; }
        public double? DPUAcres { get; set; }
        public double? GISAcres { get; set; }
        public DateTime? DPUAcresEffectiveDate { get; set; }
        public bool IsActiveInd { get; set; }
        public IEnumerable<TractUnitJunction> TractUnitJunctions { get; set; }
        public long UnitGroupId { get; set; }
        public UnitGroup UnitGroup { get; set; }
    }
}
