using Minerals.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IRoyaltyAdjustmentCardViewModelRepository
    {
        Task<IEnumerable<RoyaltyAdjustmentCardViewModel>> Get(string search);

        Task<IEnumerable<RoyaltyAdjustmentCardViewModel>> Search(RoyaltyAdjustmentCardViewModel searchCriteria);
    }
}
