using Minerals.ViewModels;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IWellRepository
    {
        Task<IEnumerable<Well>> GetAllWellsByPadAsync(long Id);
        IEnumerable<WellDataTableViewModel> GetWellsForDataTable();
        WellViewModel GetWellById(long id);
        void Update(WellViewModel model, long userId);
        WellViewModel UpdateAutoUpdatedAllowedInd(WellViewModel model);
        object UpdateAllAutoUpdatedAllowedInd(WellViewModel model);
    }
}
