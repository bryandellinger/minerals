using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Check : AuditedEntity
    {
        public long Id { get; set; }
        public string CheckNum { get; set; }
        public string Notes { get; set; }
        public string LesseeName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? CheckDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public long LesseeId { get; set; }
        public Lessee Lessee { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Royalty> Royalties { get; set; }
        public IEnumerable<StorageRental> StorageRentals { get; set; }
    }
}
