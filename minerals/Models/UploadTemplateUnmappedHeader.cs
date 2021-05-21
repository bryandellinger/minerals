using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UploadTemplateUnmappedHeader
    {
        public long Id { get; set; }
        public int Index { get; set; }
        public string Header { get; set; }
        public long UploadTemplateId { get; set; }
        public UploadTemplate UploadTemplate { get; set; }
    }
}
