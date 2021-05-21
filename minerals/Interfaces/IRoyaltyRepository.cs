using Minerals.ViewModels;
using Models;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IRoyaltyRepository
    {
        Task<Royalty> Insert(Royalty royalty);
        Task<object> GetByCheck(long id);
        Task<object> GetCheckByIdAsync(long id);
        Task<object> CheckProdMonthAsync(long lesseeId, int postMonth, int postYear, long paymentTypeId, long wellId);
        Royalty CheckProdMonth(long lesseeId, int postMonth, int postYear, long paymentTypeId, long wellId);
        void Update(RoyaltyViewModel model, long id);
    }
}
