using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface ITractLesseeJunctionRepository
    {
        Task<IEnumerable<Tract>> GetAllTractsByLesseeAsync(long Id);
    }
}
