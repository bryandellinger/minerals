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
    public class WellBoreShareRepository : IWellBoreShareRepository
    {
        private readonly DataContext context;
        public WellBoreShareRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetWellBoreSharessByWellAsync(long id)
        {
            List<WellboreShare> existingWellboreShares  = await context.WellBoreShares.Where(x => x.WellId == id).ToListAsync().ConfigureAwait(false);
            List<long> unitIds = await context.TractUnitJunctionWellJunctions.Include(x => x.TractUnitJunction).ThenInclude(x => x.Unit)
                .Where(x => x.WellId == id && x.TractUnitJunction.Unit.IsActiveInd == true )
                .Select(x => x.TractUnitJunction.UnitId)
                .Distinct()
                .ToListAsync().ConfigureAwait(false);

            List<WellboreShare> newWellboreShares = new List<WellboreShare>();
                foreach (var item in unitIds)
            {
                WellboreShare wellboreShare = existingWellboreShares.FirstOrDefault(x => x.UnitId == item && x.WellId == id);
                Unit unit = context.Units.Find(item);
                newWellboreShares.Add(new WellboreShare
                {
                    Id = wellboreShare?.Id ?? 0,
                    LengthInUnit = wellboreShare?.LengthInUnit ?? 0,
                    WellId = id,
                    UnitId = item,
                    Unit = new Unit { Id= unit.Id, UnitName = unit.UnitName, AmendmentName = unit.AmendmentName}                   
                });
            }
            return newWellboreShares;
        }
    }
}
