using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IOtherPaymentRepository
    {
        Task<object> GetAllAsync();
        Task<object> GetByCheckAsync(long id);
        void Update(OtherRental model, long id);
    }
}
