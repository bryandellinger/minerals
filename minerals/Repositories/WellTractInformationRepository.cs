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
    public class WellTractInformationRepository : IWellTractInformationRepository
    {
        private readonly DataContext context;
        public WellTractInformationRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetByLesseeAsync(long id)
        {
           List<WellTractInfo> tractInformations = await context.WellTractInformations.Where(x => x.LesseeId == id && x.ActiveInd)
               .Select(o => new WellTractInfo
               {
                   Id = o.Id,
                   WellNum = o.Well.WellNum,
                   ApiNum = o.Well.ApiNum,
                   TractNum = o.Tract.TractNum,
                   PadName = o.Pad.PadName,
                   ContractNum = o.Contract.ContractNum,
                   PercentOwnership = o.PercentOwnership,
                   RoyaltyPercent = o.RoyaltyPercent,
                   WellId = o.WellId,
                   UnitNames = string.Empty
               })
               .ToListAsync()
               .ConfigureAwait(false);

            foreach (var item in tractInformations)
            {
                var result = (from tractUnitJunctionWellJunctions in context.TractUnitJunctionWellJunctions.Where(x => x.WellId == item.WellId)
                              join tractUnitJunctions in context.TractUnitJunctions on tractUnitJunctionWellJunctions.TractUnitJunctionId equals tractUnitJunctions.Id
                              join units in context.Units on tractUnitJunctions.UnitId equals units.Id
                              select units.UnitName)
                             .OrderBy(x => x)
                             .Distinct().ToArray();

                var unitNames = String.Join(",", result);
                item.UnitNames = unitNames;
            }

            return tractInformations;
        }

        public async Task<object> GetWellTractInfoByIdAsync(long id)
        {
            WellTractInfo tractInformation = await context.WellTractInformations.Where(x => x.Id == id)
                 .Select(o => new WellTractInfo
                 {
                     Id =  o.Id,
                     WellId =  o.WellId,
                     WellNum =  o.Well.WellNum,
                     ApiNum = o.Well.ApiNum,
                     ActiveInd =o.ActiveInd,
                     TractId = o.TractId,
                     TractNum =o.Tract.TractNum,
                     PadName =o.Pad.PadName,
                     RoyaltyPercent = o.RoyaltyPercent,
                 })
                 .FirstOrDefaultAsync()
                 .ConfigureAwait(false);

            if (tractInformation != null)

            {
                var result = (from tractUnitJunctionWellJunctions in context.TractUnitJunctionWellJunctions.Where(x => x.WellId == tractInformation.WellId)
                              join tractUnitJunctions in context.TractUnitJunctions on tractUnitJunctionWellJunctions.TractUnitJunctionId equals tractUnitJunctions.Id
                              join units in context.Units on tractUnitJunctions.UnitId equals units.Id
                              select new Unit { Id = units.Id, UnitName = units.UnitName })
                              .AsEnumerable()
                              .GroupBy(x => x.Id).Select(x => x.FirstOrDefault())
                              .OrderBy(x => x.UnitName)
                              .ToList();

                tractInformation.Units = result;
            }
            return tractInformation;

        }

        public async Task<object> GetByRoyaltyAsync(long id)
        {
            WellTractInfo tractInformation =  await context.Royalties.Where(x => x.Id == id)
             .Select(o => new WellTractInfo
             {
                Id =  o.WellTractInformation.Id,
                WellId = o.WellTractInformation.WellId,
                WellNum = o.WellTractInformation.Well.WellNum,
                ApiNum = o.WellTractInformation.Well.ApiNum,
                ActiveInd = o.WellTractInformation.ActiveInd,
                TractId = o.WellTractInformation.TractId,
                TractNum =  o.WellTractInformation.Tract.TractNum,
                PadName = o.WellTractInformation.Pad.PadName,
                RoyaltyPercent = o.WellTractInformation.RoyaltyPercent,
             })
             .FirstOrDefaultAsync()
             .ConfigureAwait(false);

            if (tractInformation != null)
                
            {
                var result = (from tractUnitJunctionWellJunctions in context.TractUnitJunctionWellJunctions.Where(x => x.WellId == tractInformation.WellId)
                              join tractUnitJunctions in context.TractUnitJunctions on tractUnitJunctionWellJunctions.TractUnitJunctionId equals tractUnitJunctions.Id
                              join units in context.Units on tractUnitJunctions.UnitId equals units.Id
                              select new Unit { Id = units.Id, UnitName = units.UnitName })
                              .AsEnumerable()
                              .GroupBy(x => x.Id).Select(x => x.FirstOrDefault())
                              .OrderBy(x => x.UnitName)
                              .ToList();

                tractInformation.Units = result;                      
            }
            return tractInformation;
        }

       

        public void InsertAll(List<WellTractInformation> wellTractInformations)
        {
            context.WellTractInformations.AddRange(wellTractInformations);
            context.SaveChanges();
        }

        public void SetActiveRecordsToHistoric(long wellId, long? contractEventDetailReasonsForChangeId, DateTime changeDate)
        {

            foreach (var item in context.WellTractInformations.Where(x => x.WellId == wellId && x.ActiveInd == true))
            {

                {
                    item.ContractEventDetailReasonForChangeId = contractEventDetailReasonsForChangeId.Value;
                    item.ActiveInd = false;
                    item.ChangeDate = changeDate;
                }
            }
            context.SaveChanges();
        }

        public void UpdateOwnershipLesseeName(long id, string lesseeName )
        {
            foreach (var item in context.WellTractInformations.Where(x => x.LesseeId == id && x.ActiveInd == true))
            {
                item.LesseeName = lesseeName;
            }
            context.SaveChanges();
        }

        private class WellTractInfo
        {
            public long Id { get; internal set; }
            public string WellNum { get; internal set; }
            public string ApiNum { get; internal set; }
            public string TractNum { get; internal set; }
            public string PadName { get; internal set; }
            public string ContractNum { get; internal set; }
            public decimal? PercentOwnership { get; internal set; }
            public decimal? RoyaltyPercent { get; internal set; }
            public long WellId { get; internal set; }
            public string UnitNames { get; internal set; }
            public bool ActiveInd { get; internal set; }
            public long? TractId { get; internal set; }
            public List<Unit> Units { get; internal set; }
        }
    }
}
