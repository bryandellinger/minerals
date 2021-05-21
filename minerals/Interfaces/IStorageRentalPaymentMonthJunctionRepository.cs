using Minerals.ViewModels;
using Models;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IStorageRentalPaymentMonthJunctionRepository
    {
        Task<object> GetStorageRentalPaymentMonthsByContractAsync(long id);
        Task<object> GetStorageRentalPaymentMonthsByStorageRentalAsync(long id);
    }
}
