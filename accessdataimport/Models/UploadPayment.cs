using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UploadPayment
    {
        public long Id { get; set; }
        public DateTime? UploadPaymentEntryDate { get; set; }
        public string UploadPaymentNotes { get; set; }
        public long UploadTemplateId { get; set; }
        public UploadTemplate UploadTemplate { get; set; }
        public IEnumerable<File> Files { get; set; }
        public long? CheckId { get; set; }
        public Check Check { get; set; }
        public IEnumerable<CSVPayment> CSVPayments { get; set; }
    }
}
