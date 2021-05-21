using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Township
    {
        public long Id { get; set; }
        public string County { get; set; }
        public string Municipality { get; set; }
        public string Class { get; set; }
        public IEnumerable<TownshipLandLeaseAgreementJunction> TownshipLandLeaseAgreements { get; set; }
        public IEnumerable<TownshipSurfaceUseAgreementJunction> TownshipSurfaceUseAgreements { get; set; }
        public IEnumerable<TownshipProspectingAgreementJunction> TownshipProspectingAgreements { get; set; }
        public IEnumerable<TownshipProductionAgreementJunction> TownshipProductionAgreements { get; set; }
        public IEnumerable<TownshipSeismicAgreementJunction> TownshipSeismicAgreements { get; set; }
    }
}
