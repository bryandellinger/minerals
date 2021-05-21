using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class StorageRental
    {
        public long Id { get; set; }
        [NotMapped]
        public long? FkStorageId { get; set; }
        public DateTime? StorageRentalEntryDate { get; set; }
        public string StorageRentalNotes { get; set; }
        public string Well { get; set; }
        public int? PaymentPeriodYear { get; set; }
        public double? RentPay { get; set; }
        public long? CheckId { get; set; }
        public Check Check { get; set; }
        public long? StorageId { get; set; }
        public Storage Storage { get; set; }
        public long StorageRentalPaymentTypeId { get; set; }
        public StorageRentalPaymentType StorageRentalPaymentType { get; set;}
        public long? PeriodTypeId { get; set; }
        public PeriodType PeriodType { get; set; }

    }
}
