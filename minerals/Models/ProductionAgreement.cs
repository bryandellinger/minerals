using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ProductionAgreement : AuditedEntity
    {
        public long Id { get; set; }
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public decimal? Acreage { get; set; }
        public string AltIdInformation { get; set; }
        public long? AltIdCategoryId { get; set; }
        public AltIdCategory AltIdCategory { get; set; }
        public IEnumerable<TownshipProductionAgreementJunction> TownshipProductionAgreements { get; set; }
    }
}
