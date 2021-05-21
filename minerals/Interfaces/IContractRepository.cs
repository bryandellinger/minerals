using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IContractRepository
    {
        Contract Update(long id, Contract model);
        Task<IEnumerable<Contract>> GetContractsByWellAsync(long id);
        Contract getContractByTract(long tractid);
        Task<object> GetByStorageRentalAsync(long id);
        Task<object> GetForStorageRentalAsync();
        Task<object> GetStorageRentalInformationAsync(long id, decimal leasedTractAcreage, System.DateTime? effectiveDate, decimal? fifthYearOnwardDelayRental, decimal? secondThroughFourthYearDelayRental);
        Task<object> GetByContractRentalAsync(long id);
        Task<object> GetForContractRentalAsync(long id);
        Task<object> GetByLesseeAsync(long id);
    }
}

