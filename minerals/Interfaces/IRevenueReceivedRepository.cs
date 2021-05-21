using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IRevenueReceivedRepository
    {
        Task<IEnumerable<RevenueReceived>> GetAllAsync();
    }
}
