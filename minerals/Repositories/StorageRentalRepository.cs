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
    public class StorageRentalRepository : IStorageRentalRepository
    {
        private readonly DataContext context;
        public StorageRentalRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetAllAsync() =>
             await (from landLeaseAgreements in context.LandLeaseAgreements
                    join contracts in context.Contracts on landLeaseAgreements.ContractId equals contracts.Id
                    join storages in context.Storages on contracts.Id equals storages.ContractId
                    join storageRentals in context.StorageRentals on storages.Id equals storageRentals.StorageId
                    join periodTypes in context.PeriodTypes on storageRentals.PeriodTypeId equals periodTypes.Id
                    join storageRentalPaymentTypes in context.StorageRentalPaymentTypes on storageRentals.StorageRentalPaymentTypeId equals storageRentalPaymentTypes.Id
                    join checks in context.Checks on storageRentals.CheckId equals checks.Id
                    join lessees in context.Lessees on checks.LesseeId equals lessees.Id
                    join contractEventDetails in context.ContractEventDetails.Where(x => x.ActiveInd) on contracts.Id equals contractEventDetails.ContractId
                    select new
                    {
                        storageRentals.Id,
                        storageRentals.StorageRentalEntryDate,
                        storageRentals.StorageId,
                        contracts.ContractNum,
                        landLeaseAgreements.AltIdInformation,
                        company = lessees.LesseeName,
                        landLeaseAgreements.EffectiveDate,
                        storageRentals.RentPay,
                        checks.CheckNum,
                        checks.ReceivedDate,
                        storages.LeaseNum,
                        contractEventDetails.LesseeName,
                        storageRentals.PaymentPeriodYear,
                        periodTypes.PeriodTypeName,
                        storageRentalPaymentTypes.StorageRentalPaymentTypeName
                    }).ToListAsync()
                    .ConfigureAwait(false);

        public async Task<object> GetByCheckAsync(long id) =>
             await (from landLeaseAgreements in context.LandLeaseAgreements
                    join contracts in context.Contracts on landLeaseAgreements.ContractId equals contracts.Id
                    join storages in context.Storages on contracts.Id equals storages.ContractId
                    join storageRentals in context.StorageRentals on storages.Id equals storageRentals.StorageId
                    join periodTypes in context.PeriodTypes on storageRentals.PeriodTypeId equals periodTypes.Id
                    join storageRentalPaymentTypes in context.StorageRentalPaymentTypes on storageRentals.StorageRentalPaymentTypeId equals storageRentalPaymentTypes.Id
                    join checks in context.Checks.Where(x => x.Id == id) on storageRentals.CheckId equals checks.Id
                    join lessees in context.Lessees on checks.LesseeId equals lessees.Id
                    join contractEventDetails in context.ContractEventDetails.Where(x => x.ActiveInd) on contracts.Id equals contractEventDetails.ContractId
                    select new
                    {
                        storageRentals.Id,
                        storageRentals.StorageRentalEntryDate,
                        storageRentals.StorageId,
                        contracts.ContractNum,
                        landLeaseAgreements.AltIdInformation,
                        company = lessees.LesseeName,
                        landLeaseAgreements.EffectiveDate,
                        storageRentals.RentPay,
                        storages.LeaseNum,
                        contractEventDetails.LesseeName,
                        storageRentals.PaymentPeriodYear,
                        periodTypes.PeriodTypeName,
                        storageRentalPaymentTypes.StorageRentalPaymentTypeName
                    }).ToListAsync()
                    .ConfigureAwait(false);

        public void Update(StorageRental model, long userId)
        {
            var storageRental = context.StorageRentals.Find(model.Id);
            storageRental.StorageId = model.StorageId;
            storageRental.StorageRentalNotes = model.StorageRentalNotes;
            storageRental.CheckId = model.CheckId;
            storageRental.PeriodTypeId = model.PeriodTypeId;
            storageRental.StorageRentalPaymentTypeId = model.StorageRentalPaymentTypeId;
            storageRental.RentPay = model.RentPay;
            storageRental.StorageRentalEntryDate = model.StorageRentalEntryDate;
            storageRental.PaymentPeriodYear = model.PaymentPeriodYear;
            storageRental.Well = model.Well;
            storageRental.LastUpdateDate = DateTime.Now;
            storageRental.UpdatedBy = userId;
            context.SaveChanges();
        }
    }
}