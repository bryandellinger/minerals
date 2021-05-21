using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ApiCode
    {
        public long Id { get; set; }
        public int StateCode { get; set; }
        public string StateName { get; set; }
        public int CountyCode { get; set; }
        public string CountyName { get; set; }
    }
}
