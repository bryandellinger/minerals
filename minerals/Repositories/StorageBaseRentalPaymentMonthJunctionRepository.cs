using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class StorageBaseRentalPaymentMonthJunctionRepository : IStorageBaseRentalPaymentMonthJunctionRepository
    {
        private readonly DataContext context;

        public StorageBaseRentalPaymentMonthJunctionRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetStorageBaseRentalPaymentMonthsByContractAsync(long id) =>
        
             await(from contracts in context.Contracts.Where(x => x.Id == id)
                    join storages in context.Storages on contracts.Id equals storages.ContractId
                    join storageBaseRentalPaymentMonthJunctions in context.StorageBaseRentalPaymentMonthJunctions on storages.Id equals storageBaseRentalPaymentMonthJunctions.StorageId
                    join months in context.Months on storageBaseRentalPaymentMonthJunctions.MonthId equals months.Id
                    select new
                    {
                        months.Id
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

        public async Task<object> GetStorageBaseRentalPaymentMonthsByStorageRentalAsync(long id) =>
            await(from storageRentals in context.StorageRentals.Where(x => x.Id == id)
                    join storages in context.Storages on storageRentals.StorageId equals storages.Id
                    join storageBaseRentalPaymentMonthJunctions in context.StorageBaseRentalPaymentMonthJunctions on storages.Id equals storageBaseRentalPaymentMonthJunctions.StorageId
                    join months in context.Months on storageBaseRentalPaymentMonthJunctions.MonthId equals months.Id
                    select new
                    {
                        months.Id
                     })
                    .ToListAsync()
                    .ConfigureAwait(false);
         }
}
