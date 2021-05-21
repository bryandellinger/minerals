using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface ITractUnitJunctionRepository
    {
        Task<object> GetJunctionsByUnitAsync(long id);
        Task<object> GetJunctionByTractAsync(long id);
    }
}
