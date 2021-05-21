using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UploadTemplate : AuditedEntity
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
