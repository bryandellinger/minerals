using Models;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IApiCodeRepository
    {
        Task<object> GetStateCodes();
        Task<object> GetCountyCodes(long id);
    }
}
