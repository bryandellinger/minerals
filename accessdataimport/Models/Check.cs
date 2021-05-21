using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class Check
    {
        public long Id { get; set; }
        [NotMapped]
        public long PKCheckId { get; set; }
        [NotMapped]
        public long fkLesseeID { get; set; }
        public string CheckNum { get; set; }
        public string Notes { get; set; }
        public string LesseeName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? CheckDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public long LesseeId { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Royalty> Royalties { get; set; }
        public IEnumerable<StorageRental> StorageRentals { get; set; }
    }
}
