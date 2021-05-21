using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class TownshipSeismicAgreementJunction
    {
        public long Id { get; set; }
        public long TownshipId { get; set; }
        public Township Township { get; set; }
        public long SeismicAgreementId { get; set; }
        public SeismicAgreement SeismicAgreement { get; set; }
    }
}
