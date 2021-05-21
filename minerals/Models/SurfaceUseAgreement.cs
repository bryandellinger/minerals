using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class SurfaceUseAgreement : AuditedEntity
    {
        public long Id { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? TerminationDate { get; set; }
        public DateTime? ReversionDate { get; set; }
        public decimal? Acreage { get; set; }
        public string TerminationReason { get; set; }

        public bool? Terminated { get; set; }

        public bool? OandGSUA { get; set; }

        public bool? CoalSUA { get; set; }

        public string Notes { get; set; }

        public string AltIdInformation { get; set; }
        public long? AltIdCategoryId { get; set; }
        public AltIdCategory AltIdCategory { get; set; }

        public long ContractId { get; set; }
        public Contract Contract { get; set; }
        public long? LesseeId { get; set; }
        public Lessee Lessee { get; set; }

        public IEnumerable<TownshipSurfaceUseAgreementJunction> TownshipSurfaceUseAgreements { get; set; }

    }
}
