using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IUploadPaymentRepository
    {
        Task<object> GetAllAsync();
        void Update(UploadPayment model, long id);
        void UpdateFiles(long id, IEnumerable<File> files);
        void DeleteCSVFiles(long Id);
        void AddCSVFiles(long id, IEnumerable<CSVPayment> cSVPayments);
        Task<object> GetCSVPaymentsAsyync(long id);
    }
}
