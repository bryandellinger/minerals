using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class UploadTemplateMappedHeader
    {
        public long Id { get; set; }
        public string Label { get; set; }
        public int? Index { get; set; }
        public string Header { get; set; }
        public string Attribute { get; set; }
        public long UploadTemplateId { get; set; }
        public UploadTemplate UploadTemplate { get; set; }
    }
}
