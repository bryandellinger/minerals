using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class TblTractLessee
    {
        public long PKTractLesseeId { get; set; }
        public string FKTractId { get; set; }
        public long  FKLesseeId { get; set; }

    }
}
