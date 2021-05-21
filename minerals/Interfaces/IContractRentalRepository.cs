using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IContractRentalRepository
    {
        Task<object> GetAllAsync();
        Task<object> GetByCheckAsync(long id);
        void Update(ContractRental model, long id);
    }
}
