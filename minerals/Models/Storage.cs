using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Storage : AuditedEntity
    {
        public long Id { get; set; }
        public string StorageNotes { get; set; }
        public string LeaseNum { get; set; }
        public string StorageNotesAdditional { get; set; }
        public int? NumOfWells { get; set; }
        public bool Terminated { get; set; }
        public decimal? LeasedAcreage { get; set; }
        public decimal? InPoolLeasedAcreage { get; set; }
        public decimal? InPoolFieldAcreage { get; set; }
        public decimal? COPFieldShare { get; set; }
        public bool SubjectToInflationInd { get; set; }
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
        public IEnumerable<StorageRental> StorageRentals { get; set; }
        public decimal? StorageWellPayment { get; set; }
        public IEnumerable<StorageWellPaymentMonthJunction> StorageWellPaymentMonthJunctions { get; set; }
        public decimal? StorageBaseRentalPayment { get; set; }
        public IEnumerable<StorageBaseRentalPaymentMonthJunction> StorageBaseRentalPaymentMonthJunctions { get; set; }
        public IEnumerable<StorageRentalPaymentMonthJunction> StorageRentalPaymentMonthJunctions { get; set; }
        public decimal? StorageRentalPayment { get; set; }
    }
}
