using Minerals.ViewModels;
using Models;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IStorageWellPaymentMonthJunctionRepository
    {
        Task<object> GetStorageWellPaymentMonthsByContractAsync(long id);
        Task<object> GetStorageWellPaymentMonthsByStorageRentalAsync(long id);
    }
}
