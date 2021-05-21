using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface ITractRepository
    {
        Task<IEnumerable<Tract>> GetAllAsync();
        Task<Tract> GetByIdAsync(long id);
        Tract Update(Tract model);
        Tract UpdateAdminTract(long id);
    }
}
