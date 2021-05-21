using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class TownshipProspectingAgreementJunction
    {
        public long Id { get; set; }
        public long TownshipId { get; set; }
        public Township Township { get; set; }
        public long ProspectingAgreementId { get; set; }
        public ProspectingAgreement ProspectingAgreement { get; set; }
    }
}
