using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IStorageRentalRepository
    {
        Task<object> GetAllAsync();
        Task<object> GetByCheckAsync(long id);
        void Update(StorageRental model, long id);
    }
}
