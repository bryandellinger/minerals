using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class StorageWellPaymentMonthJunctionRepository : IStorageWellPaymentMonthJunctionRepository
    {
        private readonly DataContext context;

        public StorageWellPaymentMonthJunctionRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetStorageWellPaymentMonthsByContractAsync(long id) =>
        
             await(from contracts in context.Contracts.Where(x => x.Id == id)
                    join storages in context.Storages on contracts.Id equals storages.ContractId
                    join storageWellPaymentMonthJunctions in context.StorageWellPaymentMonthJunctions on storages.Id equals storageWellPaymentMonthJunctions.StorageId
                    join months in context.Months on storageWellPaymentMonthJunctions.MonthId equals months.Id
                    select new
                    {
                        months.Id
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

        public async Task<object> GetStorageWellPaymentMonthsByStorageRentalAsync(long id) =>
           await (from storageRentals in context.StorageRentals.Where(x => x.Id == id)
                  join storages in context.Storages on storageRentals.StorageId equals storages.Id
                  join storageWellPaymentMonthJunctions in context.StorageWellPaymentMonthJunctions on storages.Id equals storageWellPaymentMonthJunctions.StorageId
                  join months in context.Months on storageWellPaymentMonthJunctions.MonthId equals months.Id
                  select new
                  {
                      months.Id
                  })
                    .ToListAsync()
                    .ConfigureAwait(false);
    }
}
