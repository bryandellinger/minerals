using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class LandLeaseAgreement
    {
        public long Id { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal? InPoolAcreage { get; set; }
        public decimal? BufferAcreage { get; set; }
        public decimal? Acreage { get; set; }
        public decimal? NSDAcreage { get; set; }
        public bool NSDAcreageAppliesToAllInd { get; set; }
        public string AltIdInformation { get; set; }
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
        public string TerminationReason{ get; set; } 
        public long? AltIdCategoryId { get; set; }
        public AltIdCategory AltIdCategory { get; set; }
        public IEnumerable<TownshipLandLeaseAgreementJunction> TownshipLandLeaseAgreements { get; set; }
       
    }
}
