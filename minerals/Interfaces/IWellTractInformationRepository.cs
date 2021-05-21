using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IWellTractInformationRepository
    {
        public void SetActiveRecordsToHistoric(long wellId, long? contractEventDetailReasonsForChangeId, DateTime changeDate);
        void UpdateOwnershipLesseeName(long id, string lesseeName);
        void InsertAll(List<WellTractInformation> wellTractInformations);
        Task<object> GetByRoyaltyAsync(long id);
        Task<object> GetByLesseeAsync(long id);
        Task<object> GetWellTractInfoByIdAsync(long id);
    }
}
