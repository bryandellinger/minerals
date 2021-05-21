using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class TblTractOptShare
    {
        public string PKTractId { get; set; }
        public string LesseeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TrOpShare { get; set; }
        public string Horizon { get; set; }
        public bool? Operator { get; set; }
        public string Reason { get; set; }
        public decimal? Acres { get; set; }
    }
}
