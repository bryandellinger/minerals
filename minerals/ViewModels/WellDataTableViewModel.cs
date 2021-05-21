using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class WellDataTableViewModel
    {
        public long TractId { get; set; }       
        public long PadId { get; set; }
        public long WellId { get; set; }
        public string WellNum { get; set; }
        public string ApiNum { get; set; }
        public string PadName { get; set; }
        public string TractNum { get; set; }
        public string LesseeName { get; set; }
        public string Status { get; set; }
        public bool? Administrative { get; set; }
        public string AltId { get; set; }
    }
}
