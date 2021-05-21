using Microsoft.AspNetCore.Http;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IContractBusinessLogic
    {
        Tract getTractByTractNum(string tractNum, long? contractTypeId);
        void AddDistrictContractJunctions(long id, long[] districtIds);
        void DeleteAgreements(long id);
        void AddAgreement(long id, AddUpdateContractViewModel model);
        void DeleteRowContracts(long id);
        void AddRowContracts(long returnId, IEnumerable<string> rowContracts);
        void AddPaymentRequirement(long returnId, AddUpdateContractViewModel model);
        void DeleteAssociatedContracts(long id);
        void AddAssociatedContracts(long returnId, string[] v);
        void DeleteAssociatedTracts(long id);
        void AddAssociatedTracts(long returnId, string[] v);
        void DeleteCurrentEvents(long id, IEnumerable<ContractEventDetail> contractEventDetails, long reasonForChangeId, decimal? initialAcreage, decimal? currentAcreage, DateTime? lesseeEffectiveDate, string reasonForChangeDescription);
        void AddCurrentEvents(long returnId, ContractEventDetail[] contractEventDetail, DateTime? lesseeEffectiveDate, decimal? acreage);
        void UpdateWellOwnership(long id, long reasonForChangeId, DateTime? lesseeEffectiveDate);
        void UpdateStorage(long returnId,
            long? contractSubTypeId,
            Storage storage,
            IEnumerable<long> storageWellPaymentMonthIds,
            IEnumerable<long> storageBaseRentalPaymentMonthIds,
            IEnumerable<long> storageRentalPaymentMonthIds,
            long id);
        void UpdateContractRentalPaymentMonths(long returnId, IEnumerable<long> contractRentalPaymentMonthIds);
    }
}
