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
    public class SuretyRepository : ISuretyRepository
    {
        private readonly DataContext context;
        public SuretyRepository(DataContext ctx) => context = ctx;

        public void AddSuretyRiders(IEnumerable<SuretyRider> suretyRiders, long id)
        {
            List<SuretyRider> suretyRidersToAdd = new List<SuretyRider>();
            foreach (var item in suretyRiders)
            {
                suretyRidersToAdd.Add(new SuretyRider
                {
                    Id = 0,
                    SuretyId = id,
                    RiderReasonId = item.RiderReasonId,
                    EffectiveDate = item.EffectiveDate
                });
            }
            context.SuretyRiders.AddRange(suretyRidersToAdd);
            context.SaveChanges();
        }

        public void AddSuretyWells(IEnumerable<SuretyWell> suretyWells, long id)
        {
            List<SuretyWell> suretyWellsToAdd = new List<SuretyWell>();
            foreach (var item in suretyWells)
            {
                suretyWellsToAdd.Add(new SuretyWell
                {
                    Id = 0,
                    SuretyId = id,
                    WellId = item.WellId,
                    SuretyWellValue = item.SuretyWellValue
                });
            }
            context.SuretyWells.AddRange(suretyWellsToAdd);
            context.SaveChanges();
        }

        public void DeleteSuretyRiders(long id)
        {
            var suretyRidersToDelete = context.SuretyRiders.Where(x => x.SuretyId == id);
            context.SuretyRiders.RemoveRange(suretyRidersToDelete);
            context.SaveChanges();
        }

        public void DeleteSuretyWells(long id)
        {
            var suretyWellsToDelete = context.SuretyWells.Where(x => x.SuretyId == id);
            context.SuretyWells.RemoveRange(suretyWellsToDelete);
            context.SaveChanges();
        }

        public async Task<object> GetAsync() =>
       await context.Sureties.Select(x => new
       {
           x.Id,
           x.Lessee.LesseeName,
           x.Insurer,
           x.SuretyNum,
           x.SuretyStatus,
           x.SuretyType.SuretyTypeName,
           x.BondCategory.BondCategoryName,
           x.IssueDate,
           x.ReleasedDate,
           CurrentSuretyValue = x.BondCategory.BondCategoryName == Constants.BondCategoryPlugging ? x.SuretyWells.Select(y => y.SuretyWellValue).Sum() : x.CurrentSuretyValue,
           x.InitialSuretyValue,
           x.Contract.ContractNum,
           Wells = String.Join(", ", x.SuretyWells.OrderBy(x => x.Well.ApiNum).Select(x => x.Well.ApiNum))
       })
        .ToListAsync()
        .ConfigureAwait(false);

        public void Update(Surety model, long userId)
        {
            var surety = context.Sureties.Find(model.Id);
            surety.SuretyTypeId = model.SuretyTypeId;
            surety.BondCategoryId = model.BondCategoryId;
            surety.LesseeId = model.LesseeId;
            surety.ContractId = model.ContractId;
            surety.Insurer = model.Insurer;
            surety.SuretyNum = model.SuretyNum;
            surety.SuretyNotes = model.SuretyNotes;
            surety.InitialSuretyValue = model.InitialSuretyValue;
            surety.CurrentSuretyValue = model.CurrentSuretyValue;
            surety.ClaimedInd = model.ClaimedInd;
            surety.SuretyStatus = model.SuretyStatus;
            surety.ReleasedSuretyValue = model.ReleasedSuretyValue;
            surety.LastUpdateDate = DateTime.Now;
            surety.UpdatedBy = userId;
            context.SaveChanges();
        }

        public async Task<object> GetSuretiesByWellAsync(long wellId)
        {
            var s = await context.Sureties
                .Include(x => x.BondCategory)
                .Include(x => x.SuretyWells)
                .ThenInclude(x => x.Well)
                .Where(x => x.SuretyWells.Any(x => x.WellId == wellId))
                .Where(x => x.BondCategory.BondCategoryName == Constants.BondCategoryPlugging)
                .ToListAsync()
                .ConfigureAwait(false);


            return s.Select(x => new
            {
                x.Id,
                x.SuretyNum,
                x.InitialSuretyValue,
                x.IssueDate,
                x.ReleasedDate,
                x.SuretyStatus,
                x.SuretyWells.First(x => x.WellId == wellId).SuretyWellValue,
                CurrentSuretyValue = x.SuretyWells.Sum(y => y.SuretyWellValue)
            });
        }

        public async Task<object> GetSuretiesByContractAsync(long contractId) =>
            await context.Sureties
            .Include(x => x.BondCategory)
            .Where(x => x.ContractId == contractId)
            .Where(x => x.BondCategory.BondCategoryName == Constants.BondCategoryPerformance)
            .Select(x => new
            {
                x.Id,
                x.SuretyNum,
                x.InitialSuretyValue,
                x.IssueDate,
                x.ReleasedDate,
                x.SuretyStatus,
                x.CurrentSuretyValue
            })
             .ToListAsync()
             .ConfigureAwait(false);

    }
}
