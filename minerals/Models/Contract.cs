using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Contract : AuditedEntity
    {
        public long Id { get; set; }
        public string ContractNum { get; set; }
        public string Sequence { get; set; }
        public long? ContractTypeId { get; set; }
        public ContractType ContractType { get; set; }
        public long? ContractSubTypeId { get; set; }
        public ContractSubType ContractSubType { get; set; }
        public long? TractId { get; set; }
        public Tract Tract { get; set; }
        public long? PaymentRequirementId { get; set; }
        public bool? ContractNumOverride { get; set; }
        public PaymentRequirement PaymentRequirement { get; set; }
        public long? AdditionalContractInformationId { get; set; }
        public AdditionalContractInformation AdditionalContractInformation { get; set; }
        public IEnumerable<DistrictContractJunction> DistrictContacts { get; set; }
        public IEnumerable<RowContract> RowContracts { get; set; }
        public IEnumerable<AssociatedContract> AssociatedContracts { get; set; }
        public IEnumerable<AssociatedTract> AssociatedTracts { get; set; }
        public IEnumerable<ContractEventDetail> ContractEventDetails { get; set; }
        public decimal? ContractRentalPayment { get; set; }
        public bool ContractRentalPaymentOverride { get; set; }
        public IEnumerable<ContractRentalPaymentMonthJunction> ContractRentalPaymentMonthJunctions { get; set; }
    }
}
