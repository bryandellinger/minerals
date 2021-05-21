using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly DataContext context;
        public ContractRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetByStorageRentalAsync(long id) =>
        await (
                    from storageRentals in context.StorageRentals.Where(x => x.Id == id)
                    join storages in context.Storages on storageRentals.StorageId equals storages.Id
                    join contracts in context.Contracts on storages.ContractId equals contracts.Id
                    join landLeaseAgreements in context.LandLeaseAgreements on contracts.Id equals landLeaseAgreements.ContractId
                    join contractEventDetails in context.ContractEventDetails.Where(x => x.ActiveInd) on contracts.Id equals contractEventDetails.ContractId
                    select new
                    {
                        StorageId = storages.Id,
                        storages.ContractId,
                        contracts.ContractNum,
                        storages.LeaseNum,
                        landLeaseAgreements.AltIdInformation,
                        landLeaseAgreements.EffectiveDate,
                        landLeaseAgreements.TerminationDate,
                        landLeaseAgreements.ExpirationDate,
                        contractEventDetails.LesseeId,
                        contractEventDetails.LesseeName,
                        storages.StorageBaseRentalPayment,
                        storages.StorageRentalPayment,
                        storages.StorageWellPayment
                    }).FirstOrDefaultAsync()
                   .ConfigureAwait(false);

        public async Task<object> GetByContractRentalAsync(long id) =>
     await (
                 from contractRentals in context.ContractRentals.Where(x => x.Id == id)
                 join contracts in context.Contracts on contractRentals.ContractId equals contracts.Id
                 join tracts in context.Tracts on contracts.TractId equals tracts.Id
                 join landLeaseAgreements in context.LandLeaseAgreements on contracts.Id equals landLeaseAgreements.ContractId
                 join paymentRequirements in context.PaymentRequirements on contracts.PaymentRequirementId equals paymentRequirements.Id
                 select new
                 {
                     contractRentals.ContractId,
                     contracts.ContractNum,
                     tracts.TractNum,
                     contracts.ContractRentalPayment,
                     contracts.ContractRentalPaymentOverride,
                     landLeaseAgreements.AltIdInformation,
                     landLeaseAgreements.EffectiveDate,
                     landLeaseAgreements.TerminationDate,
                     landLeaseAgreements.ExpirationDate,
                     landLeaseAgreements.Acreage,
                     paymentRequirements.SecondThroughFourthYearDelayRental,
                     paymentRequirements.FifthYearOnwardDelayRental
                     
                 }).FirstOrDefaultAsync()
                .ConfigureAwait(false);

        public async Task<object> GetByLesseeAsync(long id)
        {
     var retval = await (
         from contractEventDetails in context.ContractEventDetails
                 .Where(x => x.ActiveInd)
                 .Where(x => x.LesseeId == id )
         join contracts in context.Contracts on contractEventDetails.ContractId equals contracts.Id
         join landLeaseAgreements in context.LandLeaseAgreements on contracts.Id equals landLeaseAgreements.ContractId
         join tracts in context.Tracts on contracts.TractId equals tracts.Id
         join paymentRequirements in context.PaymentRequirements on contracts.PaymentRequirementId equals paymentRequirements.Id
         select new
         {
             contractEventDetails.ContractId,
             contracts.ContractNum,
             tracts.TractNum,
             landLeaseAgreements.EffectiveDate,
             landLeaseAgreements.TerminationDate,
             landLeaseAgreements.ExpirationDate,
             landLeaseAgreements.Acreage,
             paymentRequirements.SecondThroughFourthYearDelayRental,
             paymentRequirements.FifthYearOnwardDelayRental
         })
         .ToListAsync()
         .ConfigureAwait(false);

            return retval.GroupBy(x => x.ContractId).Select(x => x.FirstOrDefault());
        }


        public async Task<object> GetForContractRentalAsync(long id)
        {
            var contractRental = await context.ContractRentals
                .Include(x => x.Check)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            long lesseeId = contractRental?.Check?.LesseeId ?? 0;

            var retval= await (
                from contractEventDetails in context.ContractEventDetails
                        .Where(x => x.ActiveInd)
                        .Where(x => lesseeId == 0 || x.LesseeId == lesseeId)
                join contracts in context.Contracts on contractEventDetails.ContractId equals contracts.Id
                join landLeaseAgreements in context.LandLeaseAgreements on contracts.Id equals landLeaseAgreements.ContractId
                join tracts in context.Tracts on contracts.TractId equals tracts.Id
                select new
                {
                    contractEventDetails.ContractId,
                    contracts.ContractNum,
                    tracts.TractNum,
                    landLeaseAgreements.EffectiveDate,
                    landLeaseAgreements.TerminationDate,
                    landLeaseAgreements.ExpirationDate,
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return retval.GroupBy(x => x.ContractId).Select(x => x.FirstOrDefault());
        }


        public async Task<object> GetForStorageRentalAsync()  =>
             await (from  storages in context.Storages
                    join contracts in context.Contracts on storages.ContractId equals contracts.Id
                    join landLeaseAgreements in context.LandLeaseAgreements on contracts.Id equals landLeaseAgreements.ContractId
                    join contractEventDetails in context.ContractEventDetails.Where(x => x.ActiveInd) on contracts.Id equals contractEventDetails.ContractId
                    select new
                    {
                        StorageId = storages.Id,
                        storages.ContractId,
                        contracts.ContractNum,
                        storages.LeaseNum,
                        landLeaseAgreements.AltIdInformation,
                        landLeaseAgreements.EffectiveDate,
                        landLeaseAgreements.TerminationDate,
                        landLeaseAgreements.ExpirationDate,
                        contractEventDetails.LesseeId,
                        contractEventDetails.LesseeName,
                        storages.StorageBaseRentalPayment,
                        storages.StorageRentalPayment,
                        storages.StorageWellPayment
                    }).ToListAsync()
                   .ConfigureAwait(false);


    public Contract getContractByTract(long tractid) =>
        context.Contracts.Include(x => x.ContractEventDetails)
        .Where(x => x.TractId == tractid)
        .Where(x => x.ContractEventDetails.Any(y => y.ActiveInd))
        .OrderByDescending(x => x.Id)
        .FirstOrDefault();


        public async Task<IEnumerable<Contract>> GetContractsByWellAsync(long id)
        {
            var well = await context.Wells.Include(x => x.Pad).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            long? tractId = well?.Pad?.TractId ?? 0;
            return await context.Contracts.Where(x => x.TractId == tractId).ToListAsync().ConfigureAwait(false);
        }

       

        public Contract Update(long id, Contract model)
        {
            Contract originalContract = context.Contracts.Include(i => i.PaymentRequirement).FirstOrDefault(i => i.Id == id);
            if (originalContract.PaymentRequirement != null)
            {
                context.PluggingSuretyDetails.RemoveRange(context.PluggingSuretyDetails.Where(x => x.PaymentRequirementId == originalContract.PaymentRequirement.Id));
                context.AdditionalBonuses.RemoveRange(context.AdditionalBonuses.Where(x => x.PaymentRequirementId == originalContract.PaymentRequirement.Id));
            } else
            {
                originalContract.PaymentRequirement = new PaymentRequirement() { Id = 0 };
            }        
            originalContract.ContractNum = model.ContractNum;
            originalContract.LastUpdateDate = model.LastUpdateDate;
            originalContract.UpdatedBy = model.UpdatedBy;
            originalContract.TractId = model.TractId;
            originalContract.ContractTypeId = model.ContractTypeId;
            originalContract.ContractSubTypeId = model.ContractSubTypeId;
            originalContract.Sequence = model.Sequence;
            originalContract.AdditionalContractInformationId = model.AdditionalContractInformationId;
            originalContract.ContractNumOverride = model.ContractNumOverride;
            originalContract.PaymentRequirement.CheckSubmissionPeriod = model.PaymentRequirement.CheckSubmissionPeriod;
            originalContract.PaymentRequirement.FirstYearRentalBonusAmount = model.PaymentRequirement.FirstYearRentalBonusAmount;
            originalContract.PaymentRequirement.TotalBonusAmount = model.PaymentRequirement.TotalBonusAmount;
            originalContract.PaymentRequirement.LeaseExtensionBonus = model.PaymentRequirement.LeaseExtensionBonus;
            originalContract.PaymentRequirement.RentalShutInDueDate = model.PaymentRequirement.RentalShutInDueDate;
            originalContract.PaymentRequirement.SecondThroughFourthYearDelayRental = model.PaymentRequirement.SecondThroughFourthYearDelayRental;
            originalContract.PaymentRequirement.SecondThroughFourthYearShutInRate = model.PaymentRequirement.SecondThroughFourthYearShutInRate;
            originalContract.PaymentRequirement.FifthYearOnwardDelayRental = model.PaymentRequirement.FifthYearOnwardDelayRental;
            originalContract.PaymentRequirement.FifthYearOnwardShutInRate = model.PaymentRequirement.FifthYearOnwardShutInRate;
            originalContract.PaymentRequirement.ShutInPaymentInterval = model.PaymentRequirement.ShutInPaymentInterval;
            originalContract.PaymentRequirement.SecondThroughFourthYearShutInRateInterval = model.PaymentRequirement.SecondThroughFourthYearShutInRateInterval;
            originalContract.PaymentRequirement.StorageFee = model.PaymentRequirement.StorageFee;
            originalContract.PaymentRequirement.AllowableVariancePerAuditFieldPercent = model.PaymentRequirement.AllowableVariancePerAuditFieldPercent;
            originalContract.PaymentRequirement.ProducerPriceIndex = model.PaymentRequirement.ProducerPriceIndex;
            originalContract.PaymentRequirement.FiveYearInflationIntervalPeriodInd = model.PaymentRequirement.FiveYearInflationIntervalPeriodInd;
            originalContract.PaymentRequirement.TestOfWellEconomy = model.PaymentRequirement.TestOfWellEconomy;
            originalContract.PaymentRequirement.PerformanceSurety = model.PaymentRequirement.PerformanceSurety;
            originalContract.PaymentRequirement.PluggingSuretyDetails = model.PaymentRequirement.PluggingSuretyDetails;
            originalContract.PaymentRequirement.AgreementFee = model.PaymentRequirement.AgreementFee;
            originalContract.PaymentRequirement.AdditionalBonuses = model.PaymentRequirement.AdditionalBonuses;
            originalContract.ContractRentalPayment = model.ContractRentalPayment;
            originalContract.ContractRentalPaymentOverride = model.ContractRentalPaymentOverride;
            context.SaveChanges();
            return originalContract;
        }

        public async Task<object> GetStorageRentalInformationAsync(
            long id, decimal leasedTractAcreage, DateTime? effectiveDate,
            decimal? fifthYearOnwardDelayRental, decimal? secondThroughFourthYearDelayRental)
        {
            List<WellTractInformation> productingWellTractInformations = await (
                context.WellTractInformations
                .Include(x  => x.Well)
                         .ThenInclude(x => x.WellStatus)
                .Include(x => x.Well)
                        .ThenInclude(x => x.TractUnitJunctionWellJunctions)
                            .ThenInclude(x => x.TractUnitJunction)
                                .ThenInclude(x => x.Unit)
                .Where(x => x.ContractId == id))
                .Where(x => x.ActiveInd)
                .Where(x => x.Well.WellStatus.WellStatusName == Constants.WellStatusProducing)
                .ToListAsync()
                .ConfigureAwait(false);

            List<WellTractInformation> distinctProducingWellTractInformations =
                  productingWellTractInformations.GroupBy(x => x.Well.Id).Select(x => x.FirstOrDefault()).ToList();

            int numberOfProducingUnitizedWells = 0;
            List<TractUnitJunctionInfo> tractUnitJunctionInfos= new List<TractUnitJunctionInfo>();
            double totaNonUnitizedAcreageAttributable = 0;
            int nonUnitizedCounter = 0;

            foreach (var item in distinctProducingWellTractInformations)
            {
                if (item.Well.TractUnitJunctionWellJunctions.Any())
                {
                    numberOfProducingUnitizedWells++;
                    foreach (var tujwj in item.Well.TractUnitJunctionWellJunctions)
                    {
                        tractUnitJunctionInfos.Add(new TractUnitJunctionInfo
                            {
                         id = tujwj.Id,
                         wellid = tujwj.WellId,
                          copaAcres =   tujwj.TractUnitJunction.COPAcres??0,
                          numOfWells = GetNumOfWells(tujwj.TractUnitJunction.UnitId)
                            //numOfWells = item.Well.TractUnitJunctionWellJunctions
                            //.GroupBy(x => x.WellId)
                            //.Select(x => x.FirstOrDefault())
                            //.Count()
                        });
                    }
                }
                else
                {
                    nonUnitizedCounter++;
                    if (item.Well.AcreageAttributableToWells != null)
                    {
                        totaNonUnitizedAcreageAttributable =
                       totaNonUnitizedAcreageAttributable + item.Well.AcreageAttributableToWells.Value;
                    }

                }
            }

            tractUnitJunctionInfos = tractUnitJunctionInfos.GroupBy(x => x.wellid).Select(x => x.FirstOrDefault()).ToList();

            var totalYears = (DateTime.Now - (effectiveDate == null ? DateTime.Now : effectiveDate.Value)).TotalDays / 365;

            var retval = new { 
                                NumberOfProducingWells = distinctProducingWellTractInformations.Count,
                                NumberOfProducingUnitizedWells = numberOfProducingUnitizedWells,
                                NumberOfProducingNonUnitizedWells =
                                    distinctProducingWellTractInformations.Count - numberOfProducingUnitizedWells,
                                NonUnizedAverageAcreageAttributable = nonUnitizedCounter > 0 ?
                                    totaNonUnitizedAcreageAttributable / nonUnitizedCounter : 0,
                                NonUnitizedTotalAcreageHeldByProduction= totaNonUnitizedAcreageAttributable,
                                UnitizedTotalAcreageHeldByProduction = tractUnitJunctionInfos
                                                                                                   .Sum(x => x.AcreageAttributable),
                                AllTotalAcreageAttributable = totaNonUnitizedAcreageAttributable + tractUnitJunctionInfos
                                                                                                   .Sum(x => x.AcreageAttributable),
                                LeasedTractAcreage = leasedTractAcreage,
                                AcreageToApplyToRentalCalculation = leasedTractAcreage - (decimal)(totaNonUnitizedAcreageAttributable + tractUnitJunctionInfos
                                                                                                   .Sum(x => x.AcreageAttributable)),
                                RentPerAcre = totalYears > 5 ? fifthYearOnwardDelayRental??0 : secondThroughFourthYearDelayRental??0,
                                RentalPaymentDue = (totalYears > 5 ? fifthYearOnwardDelayRental ?? 0 : secondThroughFourthYearDelayRental ?? 0)
                                                                   *
                                                                   (leasedTractAcreage - (decimal)(totaNonUnitizedAcreageAttributable + tractUnitJunctionInfos
                                                                                                   .Sum(x => x.AcreageAttributable)))
            };

            return retval;
             
        }

        private int GetNumOfWells(long unitId) {
            {
                var x = (from units in context.Units.Where(x => x.Id == unitId)
                         join tractUnitJunctions in context.TractUnitJunctions on units.Id equals tractUnitJunctions.UnitId
                         join tractUnitJunctionWellJunctions in context.TractUnitJunctionWellJunctions on tractUnitJunctions.Id equals tractUnitJunctionWellJunctions.TractUnitJunctionId
                         join wells in context.Wells on tractUnitJunctionWellJunctions.WellId equals wells.Id
                         join wellStatuses in context.WellStatuses.Where(x => x.WellStatusName == Constants.WellStatusProducing) on wells.WellStatusId equals wellStatuses.Id
                         select wells.Id).ToList();


                return x.Distinct().Count();
            }



                   
        }

        private class TractUnitJunctionInfo
        {
            internal long id;
            internal long wellid;

            public double copaAcres { get; internal set; }

            public int numOfWells { get; internal set; }

            public double  AcreageAttributable
            {
                get
                {
                    return numOfWells > 0  ? copaAcres / numOfWells : 0;
                }
            }

        }
    }
}
