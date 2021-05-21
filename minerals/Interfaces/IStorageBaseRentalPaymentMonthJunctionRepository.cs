using Minerals.ViewModels;
using Models;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IStorageBaseRentalPaymentMonthJunctionRepository
    {
        Task<object> GetStorageBaseRentalPaymentMonthsByContractAsync(long id);
        Task<object> GetStorageBaseRentalPaymentMonthsByStorageRentalAsync(long id);
    }
}
