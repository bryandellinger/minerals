using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly DataContext context;
        public StorageRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetByContractAsync(long id) =>
        await context.Storages.Where(x => x.ContractId == id).FirstOrDefaultAsync().ConfigureAwait(false);

        public void Update(Storage storage,
            IEnumerable<long> storageWellPaymentMonthIds,
            IEnumerable<long> storageBaseRentalPaymentMonthIds,
            IEnumerable<long> storageRentalPaymentMonthIds,
            long userId)
        {
            var monthsToDelete = context.StorageWellPaymentMonthJunctions.Where(x => x.StorageId == storage.Id);
            context.StorageWellPaymentMonthJunctions.RemoveRange(monthsToDelete);
            context.SaveChanges();

            var monthsToDelete2 = context.StorageBaseRentalPaymentMonthJunctions.Where(x => x.StorageId == storage.Id);
            context.StorageBaseRentalPaymentMonthJunctions.RemoveRange(monthsToDelete2);
            context.SaveChanges();

            var monthsToDelete3 = context.StorageRentalPaymentMonthJunctions.Where(x => x.StorageId == storage.Id);
            context.StorageRentalPaymentMonthJunctions.RemoveRange(monthsToDelete3);
            context.SaveChanges();

            List<StorageWellPaymentMonthJunction> storageWellPaymentMonthJunctions = new List<StorageWellPaymentMonthJunction>();
            foreach (long monthId in storageWellPaymentMonthIds)
            {
                storageWellPaymentMonthJunctions.Add(new StorageWellPaymentMonthJunction { MonthId = monthId, StorageId = storage.Id, Id = 0 });
            }

            List<StorageBaseRentalPaymentMonthJunction> storageBaseRentalPaymentMonthJunctions = new List<StorageBaseRentalPaymentMonthJunction>();
            foreach (long monthId in storageBaseRentalPaymentMonthIds)
            {
                storageBaseRentalPaymentMonthJunctions.Add(new StorageBaseRentalPaymentMonthJunction { MonthId = monthId, StorageId = storage.Id, Id = 0 });
            }

            List<StorageRentalPaymentMonthJunction> storageRentalPaymentMonthJunctions = new List<StorageRentalPaymentMonthJunction>();
            foreach (long monthId in storageRentalPaymentMonthIds)
            {
                storageRentalPaymentMonthJunctions.Add(new StorageRentalPaymentMonthJunction { MonthId = monthId, StorageId = storage.Id, Id = 0 });
            }

            var strg = context.Storages.Find(storage.Id);
            strg.StorageWellPayment = storage.StorageWellPayment;
            strg.StorageBaseRentalPayment = storage.StorageBaseRentalPayment;
            strg.StorageRentalPayment = storage.StorageRentalPayment;
            strg.SubjectToInflationInd = storage.SubjectToInflationInd;
            strg.StorageWellPaymentMonthJunctions = storageWellPaymentMonthJunctions;
            strg.StorageBaseRentalPaymentMonthJunctions = storageBaseRentalPaymentMonthJunctions;
            strg.StorageRentalPaymentMonthJunctions = storageRentalPaymentMonthJunctions;
            strg.LeaseNum = storage.LeaseNum;
            strg.LastUpdateDate = DateTime.Now;
            strg.UpdatedBy = userId;

            context.SaveChanges();
        }
    }
}
