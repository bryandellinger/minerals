using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class File
    {
        public long Id { get; set; }
        public Guid FileGuid { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string FileIcon { get; set; }
    }
}
