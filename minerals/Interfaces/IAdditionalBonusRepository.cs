using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IAdditionalBonusRepository
    {
        Task<object> GetByContractAsync(long id);
    }
}
