using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class TownshipSurfaceUseAgreementJunction
    {
        public long Id { get; set; }
        public long TownshipId { get; set; }
        public Township Township { get; set; }

        public long SurfaceUseAgreementId { get; set; }
        public SurfaceUseAgreement SurfaceUseAgreement { get; set; }
    }
}
