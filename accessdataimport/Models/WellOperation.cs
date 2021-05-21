using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class WellOperation
    {
        public long Id { get; set; }

        [NotMapped]
        public long PKWellOpsId { get; set; }
        public Single? OpShare { get; set; }
        public  bool? ReportsTotal { get; set; }
        public string Notes { get; set; }
        public long? LesseeId { get; set; }
        public Lessee Lessee { get; set; }
        public long? WellId { get; set; }
        public Well Well { get; set; }

    }
}
