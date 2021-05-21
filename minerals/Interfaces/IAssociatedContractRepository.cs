using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IAssociatedContractRepository
    {
        Task<IEnumerable<AssociatedContract>> GetAllAssociatedContractsByContractAsync(long id);
        List<String>  GetAssociatedContractsDropDownList();
    }
}
