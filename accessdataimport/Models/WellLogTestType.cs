using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
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
