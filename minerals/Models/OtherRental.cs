﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class OtherRental : AuditedEntity
    {
        public long Id { get; set; }
        public DateTime? OtherRentalEntryDate { get; set; }
        public string OtherRentalNotes { get; set; }
        public double? OtherRentPay { get; set; }
        public string OtherPaymentType { get; set; }
        public long? CheckId { get; set; }
        public Check Check { get; set; }
        
    }
}
