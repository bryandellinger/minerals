
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Pad
    {
        public long Id { get; set; }

        [NotMapped]
        public long PKPadId { get; set; }
        public string PadName { get; set; }
        public long TractId { get; set; }
        public Tract Tract { get; set; }

        public IEnumerable<Well> Wells { get; set; }

    }
}
