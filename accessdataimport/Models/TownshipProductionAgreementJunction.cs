using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class TownshipProductionAgreementJunction
    {
        public long Id { get; set; }
        public long TownshipId { get; set; }
        public Township Township { get; set; }
        public long ProductionAgreementId { get; set; }
        public ProductionAgreement ProductionAgreement { get; set; }
    }
}
