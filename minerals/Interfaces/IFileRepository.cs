using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IFileRepository
    {
        Task<object> GetFilesByUnitAsync(long id);
        Task<object> GetFilesByCheckAsync(long id);
        Task<object> GetFilesByUploadTemplateAsync(long id);
        Task<object> GetFilesByUploadPaymentAsync(long id);
    }
}
