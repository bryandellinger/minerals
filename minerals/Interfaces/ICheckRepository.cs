using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Minerals.Interfaces
{
    public interface ICheckRepository
    {
        void Update(Check model, long userId);
        void UpdateFiles(long id, IEnumerable<File> files);
        Task<object> GetSelect2Data(string id);
        Task<object> GetCheckByStorageRentalAsync(long id);
        Task<object> GetCheckByContractRentalAsync(long id);
        Task<object> GetCheckByOtherPaymentAsync(long id);
        Task<object> GetCheckByUploadPaymentAsync(long id);
    }
}
