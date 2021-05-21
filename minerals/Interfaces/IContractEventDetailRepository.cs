using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IContractEventDetailRepository
    {
        Task<IEnumerable<ContractEventDetail>> GetContractEventDetailsByContractAsync(long id);
        void UpdateCurrentContractEventDetailLeseeNamesByLesseeId(long lesseeId, string lesseeName);
        void InsertAll(List<ContractEventDetail> contractEventDetails);
        void DeleteAll(List<ContractEventDetail> currentEventDetailsToRemove);
    }
}
