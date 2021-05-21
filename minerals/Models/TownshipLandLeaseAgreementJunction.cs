using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class TownshipLandLeaseAgreementJunction
    {
        public long Id { get; set; }

        public long TownshipId { get; set; }
        public Township Township { get; set; }

        public long LandLeaseAgreementId { get; set; }
        public LandLeaseAgreement LandLeaseAgreement { get; set; }
    }
}
