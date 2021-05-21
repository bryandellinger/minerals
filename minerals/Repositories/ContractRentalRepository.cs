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
    public class ContractRentalRepository : IContractRentalRepository
    {
        private readonly DataContext context;
        public ContractRentalRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetAllAsync() {

            var retval1 = from contractRentals in context.ContractRentals
                          join contracts in context.Contracts
                           on contractRentals.ContractId equals contracts.Id
                          join landLeaseAgreements in context.LandLeaseAgreements
                          on contracts.Id equals landLeaseAgreements.ContractId
                          join checks in context.Checks
                          on contractRentals.CheckId equals checks.Id
                          join tracts in context.Tracts
                          on contracts.TractId equals tracts.Id
                          join lessees in context.Lessees
                          on checks.LesseeId equals lessees.Id
                          join periodTypes in context.PeriodTypes
                          on contractRentals.PeriodTypeId equals periodTypes.Id
                          select new
                          {
                              contractRentals.Id,
                              contractRentals.ContractPaymentPeriodYear,
                              contractRentals.ContractRentalEntryDate,
                              contractRentals.ContractRentPay,
                              contractRentals.HeldByProduction,
                              contracts.ContractNum,
                              landLeaseAgreements.EffectiveDate,
                              landLeaseAgreements.ExpirationDate,
                              landLeaseAgreements.TerminationDate,
                              checks.CheckNum,
                              checks.CheckDate,
                              tracts.TractNum,
                              lessees.LesseeName,
                              periodTypes.PeriodTypeName
                          };

            var retval2 = from contractRentals in context.ContractRentals.Where(x => x.CheckId == null)
                          join contracts in context.Contracts
                           on contractRentals.ContractId equals contracts.Id
                          join landLeaseAgreements in context.LandLeaseAgreements
                          on contracts.Id equals landLeaseAgreements.ContractId
                          join tracts in context.Tracts
                          on contracts.TractId equals tracts.Id
                          join periodTypes in context.PeriodTypes
                          on contractRentals.PeriodTypeId equals periodTypes.Id
                          select new
                          {
                              contractRentals.Id,
                              contractRentals.ContractPaymentPeriodYear,
                              contractRentals.ContractRentalEntryDate,
                              contractRentals.ContractRentPay,
                              contractRentals.HeldByProduction,
                              contracts.ContractNum,
                              landLeaseAgreements.EffectiveDate,
                              landLeaseAgreements.ExpirationDate,
                              landLeaseAgreements.TerminationDate,
                             CheckNum = string.Empty,
                             CheckDate = (DateTime?)null,
                              tracts.TractNum,
                              LesseeName = string.Empty,
                              periodTypes.PeriodTypeName
                          };

            return await (retval1.Union(retval2).ToListAsync().ConfigureAwait(false));

        }

        public async Task<object> GetByCheckAsync(long id)  =>
            await(from contractRentals in context.ContractRentals
                  join contracts in context.Contracts
                   on contractRentals.ContractId equals contracts.Id
                  join landLeaseAgreements in context.LandLeaseAgreements
                  on contracts.Id equals landLeaseAgreements.ContractId
                  join checks in context.Checks.Where(x => x.Id == id)
                  on contractRentals.CheckId equals checks.Id
                  join tracts in context.Tracts
                  on contracts.TractId equals tracts.Id
                  join lessees in context.Lessees
                  on checks.LesseeId equals lessees.Id
                  join periodTypes in context.PeriodTypes
                  on contractRentals.PeriodTypeId equals periodTypes.Id
                  select new
                  {
                      contractRentals.Id,
                      contractRentals.ContractPaymentPeriodYear,
                      contractRentals.ContractRentalEntryDate,
                      contractRentals.ContractRentPay,
                      contractRentals.HeldByProduction,
                      contracts.ContractNum,
                      landLeaseAgreements.EffectiveDate,
                      landLeaseAgreements.ExpirationDate,
                      landLeaseAgreements.TerminationDate,
                      checks.CheckNum,
                      checks.CheckDate,
                      tracts.TractNum,
                      lessees.LesseeName,
                      periodTypes.PeriodTypeName
                  })
               .ToListAsync()
          .ConfigureAwait(false);

        public void Update(ContractRental model, long userId)
        {
            var contractRental = context.ContractRentals.Find(model.Id);
            contractRental.ContractId = model.ContractId;
            contractRental.ContractRentalNotes = model.ContractRentalNotes;
            contractRental.CheckId = model.CheckId;
            contractRental.PeriodTypeId = model.PeriodTypeId;
            contractRental.ContractRentPay = model.ContractRentPay;
            contractRental.ContractRentalEntryDate = model.ContractRentalEntryDate;
            contractRental.ContractPaymentPeriodYear = model.ContractPaymentPeriodYear;
            contractRental.HeldByProduction = model.HeldByProduction;
            contractRental.LastUpdateDate = DateTime.Now;
            contractRental.UpdatedBy = userId;
            context.SaveChanges();
        }
    }
}
