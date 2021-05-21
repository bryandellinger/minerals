using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
   public class Lessee
    {
        public long Id { get; set; }

        public long? PKLesseeId { get; set; }

        public string LesseeName { get; set; }

        public bool? LogicalDeleteIn { get; set; }

       
        public IEnumerable<TractLesseeJunction> TractLessee { get; set; }

        public IEnumerable<LesseeContact> LesseeContacts { get; set; }
        public IEnumerable<LesseeHistory> LesseeHistories { get; set; }
        public IEnumerable<ContractEventDetail> ContractEventDetails { get; set; }
    }
}
