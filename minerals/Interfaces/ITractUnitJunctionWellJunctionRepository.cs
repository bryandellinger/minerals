using Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Minerals.Interfaces
{
    public interface ITractUnitJunctionWellJunctionRepository
    {
        Task<object> GetUnitsByWell(long id);
        Task<object> GetUnitsAndTractsByWell(long id);
        Task<object> GetNriTractInfobywell(long id);
    }
}
