using System;

namespace Models
{
    public class WellOperation
    {
        public long Id { get; set; }
        public Single? OpShare { get; set; }
        public bool? ReportsTotal { get; set; }
        public string Notes { get; set; }
        public long? LesseeId { get; set; }
        public Lessee Lessee { get; set; }
        public long? WellId { get; set; }
        public Well Well { get; set; }

    }
}
