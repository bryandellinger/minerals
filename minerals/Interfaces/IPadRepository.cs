using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IPadRepository
    {
        Task<IEnumerable<Pad>> GetAllPadsByTractAsync(long Id);
    }
}
