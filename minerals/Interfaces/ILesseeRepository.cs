using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface ILesseeRepository
    {
        Lessee Update(long id, EditLesseeViewModel model, User user);
        Task<object> GetByPaymentIdAsync(long id);
        Task<object> GetByStorageRental(long id);
        Task<object> GetByContractRental(long id);
        Task<object> GetByOtherPayment(long id);
        Task<object> GetByUploadPayment(long id);
    }
}
