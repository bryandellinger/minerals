using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IContractRentalPaymentMonthJunctionRepository
    {
        Task<object> GetContractRentalPaymentMonthsByContractAsync(long id);
        Task<object> GetContractRentalPaymentMonthsByContractRentalAsync(long id);
    }
}
