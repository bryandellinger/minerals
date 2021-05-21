using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface ILesseeBusinessLogic
    {
        void CreateHistoricalContractEventDetailRecords(long id);
        void CreateHistoricalWellOwnershipRecords(long id, bool addHistoryInd);
    }
}
