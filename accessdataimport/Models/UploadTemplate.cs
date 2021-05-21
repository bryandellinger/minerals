using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class UploadTemplate
    {
        public long Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateNotes { get; set; }
        public long LesseeId { get; set; }
        public Lessee Lessee { get; set; }
        public IEnumerable<File> Files { get; set; }

        public IEnumerable<UploadTemplateMappedHeader> UploadTemplateMappedHeaders { get; set; }
        public IEnumerable<UploadTemplateUnmappedHeader> UploadTemplateUnmappedHeaders { get; set; }
        public IEnumerable<UploadPayment> UploadPayments { get; set; }
    }
}
