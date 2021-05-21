using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IRoyaltyAdjustmentRepository
    {
        Task<object> GetByPaymentAsync(long id);
        Task<object> GetByCheckAsync(long id);
        RoyaltyAdjustment Update(RoyaltyAdjustment model, long id1, long id2);
        void Delete(long id);
    }
}
