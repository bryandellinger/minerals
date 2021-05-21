using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class AdditionalBonus
    {
        public long Id { get; set; }
        public decimal AdditionalBonusAmount { get; set; }
        public long PaymentRequirementId { get; set; }
        public PaymentRequirement PaymentRequirement { get; set; }
    }
}
