using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface ISuretyRepository
    {
        Task<object> GetAsync();
        void Update(Surety model, long id);
        void DeleteSuretyRiders(long id);
        void AddSuretyRiders(IEnumerable<SuretyRider> suretyRiders,  long id);
        void AddSuretyWells(IEnumerable<SuretyWell> suretyWells, long id);
        void DeleteSuretyWells(long id);
        Task<object> GetSuretiesByWellAsync(long id);
        Task<object> GetSuretiesByContractAsync(long id);
    }
}
