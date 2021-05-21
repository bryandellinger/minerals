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

    public class TractUnitJunctionRepository : ITractUnitJunctionRepository
    {
        private readonly DataContext context;
        public TractUnitJunctionRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetJunctionByTractAsync(long id)
        {
            var availableWellIds = await context.WellTractInformations
                    .Where(x => x.TractId != null && x.TractId.Value == id)
                    .Select(x => x.WellId)
                    .ToArrayAsync().ConfigureAwait(false);

            return new TractUnitJunctionViewModel
            {
                Id = 0,
                UnitId = 0,
                TractId = id,
                WellIds = Array.Empty<long>(),
                AvailableWellIds = availableWellIds
            };
        }


        public async Task<object> GetJunctionsByUnitAsync(long id)
        {
            var wellTractInformations = await context.WellTractInformations
                .Where(x => x.TractId != null)
                .ToListAsync().ConfigureAwait(false);

           List<TractUnitJunctionViewModel> tractUnitJunctions =  await context.TractUnitJunctions
            .Where(x => x.UnitId == id)
            .Select(x => new TractUnitJunctionViewModel
            {
                Id = x.Id,
                UnitId = x.UnitId,
                TractId = x.TractId,
                COPAcres = x.COPAcres,
                WellIds = x.TractUnitJunctionWellJunctions.Select(z => z.WellId).ToArray()
            }).ToListAsync().ConfigureAwait(false);

            foreach (var item in tractUnitJunctions)
            {
                item.AvailableWellIds = wellTractInformations
                    .Where(x => x.TractId.Value == item.TractId)
                    .Select(x => x.WellId)
                    .ToArray();
            }

            return tractUnitJunctions;
        }

        private class TractUnitJunctionViewModel
        {
            public long Id { get; set; }
            public long UnitId { get; set; }
            public long TractId { get; set; }
            public double? COPAcres { get; set; }
            public long[] WellIds { get; set; }
            public long[] AvailableWellIds { get; set; }
        }
    }
}
