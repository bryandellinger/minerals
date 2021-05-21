using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class RevenueReceived
    {
        public long Id { get; set; }

        [NotMapped]
        public long PKCheckId { get; set; }

        public DateTime? RecvDate { get; set; }

        public string CheckNum { get; set; }

        public DateTime? CheckDate { get; set; }

        public decimal? RecvCheckTotal { get; set; }

        public string RTNum { get; set; }

        public string DocNum { get; set; }

        public DateTime? ReverseTransDate { get; set; }

        public DateTime? PostedDate { get; set; }

        public string Notes { get; set; }

        public string ControlNum { get; set; }

        public bool? CheckReturned { get; set; }

        public int? FloorPriceAudit { get; set; }

        public int? NRIAudit { get; set; }

        public int? Over90DayAudit { get; set; }

        public long? LesseeId { get; set; }

        [ForeignKey("LesseeId ")]
        public Lessee Lessee { get; set; }

    }
}
