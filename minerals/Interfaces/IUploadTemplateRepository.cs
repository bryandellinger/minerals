using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IUploadTemplateRepository
    {
        void UpdateFiles(long id, IEnumerable<File> files);
        void Update(UploadTemplate model, long id);
        Task<object> GetAsync();
        Task<object> GetHeadersAsync(long id);
    }
}
