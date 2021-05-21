using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IStorageRepository
    {
        Task<object> GetByContractAsync(long id);

        void Update(Storage storage,
            IEnumerable<long> storageWellPaymentMonthIds, 
            IEnumerable<long> storageBaseRentalPaymentMonthIds,
            IEnumerable<long> storageRentalPaymentMonthIds,
            long userId);
    }
}
