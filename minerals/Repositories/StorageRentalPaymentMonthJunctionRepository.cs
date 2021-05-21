using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class StorageRentalPaymentMonthJunctionRepository : IStorageRentalPaymentMonthJunctionRepository
    {
        private readonly DataContext context;

        public StorageRentalPaymentMonthJunctionRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetStorageRentalPaymentMonthsByContractAsync(long id) =>
        
             await(from contracts in context.Contracts.Where(x => x.Id == id)
                    join storages in context.Storages on contracts.Id equals storages.ContractId
                    join storageRentalPaymentMonthJunctions in context.StorageRentalPaymentMonthJunctions on storages.Id equals storageRentalPaymentMonthJunctions.StorageId
                    join months in context.Months on storageRentalPaymentMonthJunctions.MonthId equals months.Id
                    select new
                    {
                        months.Id
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

        public  async Task<object> GetStorageRentalPaymentMonthsByStorageRentalAsync(long id) =>
     await (from storageRentals in context.StorageRentals.Where(x => x.Id == id)
            join storages in context.Storages  on storageRentals.StorageId equals storages.Id
            join storageRentalPaymentMonthJunctions in context.StorageRentalPaymentMonthJunctions on storages.Id equals storageRentalPaymentMonthJunctions.StorageId
            join months in context.Months on storageRentalPaymentMonthJunctions.MonthId equals months.Id
            select new
            {
                months.Id
            })
                    .ToListAsync()
                    .ConfigureAwait(false);
    }
}
