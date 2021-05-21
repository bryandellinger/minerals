using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class CreateCheckViewModel
    {
        public DateTime CheckDate { get; set; }
        public string CheckNum { get; set; }
        public long FileId { get; set; }
        public long  UploadTemplateId { get; set; }         
    }
}
