using Minerals.ViewModels;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IUnitRepository
    {
        Task<object> GetUnitsForDataTableAsync();
        void Update(UnitViewModel model, long userId);
        void AddAmendment(UnitViewModel model, long userId);
        Task<object> GetUnitGroupInfoByUnit(long id);
        void UpdateFiles(long id, IEnumerable<File> files);
        List<Tract> GetTractsByUnit(long id);
        List<Well> GetWellsByUnit(long id);
    }
}
