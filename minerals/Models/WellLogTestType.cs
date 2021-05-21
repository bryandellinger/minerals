using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class WellLogTestType
    {
        public long Id { get; set; }
        public string WellLogTestName { get; set; }
        public IEnumerable<DigitalImageWellLogTestTypeWellJunction> DigitaImagelWellLogTestTypeWellJunctions { get; set; }
        public IEnumerable<DigitalWellLogTestTypeWellJunction> DigitalWellLogTestTypeWellJunctions { get; set; }
        public IEnumerable<HardCopyWellLogTestTypeWellJunction> HardCopyWellLogTestTypeWellJunctions { get; set; }

    }
}
