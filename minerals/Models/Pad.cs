using System.Collections.Generic;

namespace Models
{
    public class Pad
    {
        public long Id { get; set; }

        public string PadName { get; set; }

        public long TractId { get; set; }
        public Tract Tract { get; set; }

        public IEnumerable<Well> Wells { get; set; }

    }
}
