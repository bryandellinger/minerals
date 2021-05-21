using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ContractRental : AuditedEntity
    {
        public long Id { get; set; }
        public DateTime? ContractRentalEntryDate { get; set; }
        public string ContractRentalNotes { get; set; }
        public int? ContractPaymentPeriodYear { get; set; }
        public double? ContractRentPay { get; set; }
        public bool HeldByProduction { get; set; }
        public long? CheckId { get; set; }
        public Check Check { get; set; }
        public long? PeriodTypeId { get; set; }
        public PeriodType PeriodType { get; set; }
        public long ContractId { get; set; }
        public Contract Contract{ get; set; }
        
    }
}
