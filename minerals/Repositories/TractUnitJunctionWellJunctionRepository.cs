using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class TractUnitJunctionWellJunctionRepository : ITractUnitJunctionWellJunctionRepository
    {
        private readonly DataContext context;
        public TractUnitJunctionWellJunctionRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetUnitsByWell(long id) =>
            await (from w in context.Wells.Where(x => x.Id == id)
                   join tuwj in context.TractUnitJunctionWellJunctions on w.Id equals tuwj.WellId
                   join tuj in context.TractUnitJunctions on tuwj.TractUnitJunctionId equals tuj.Id
                   join u in context.Units.Where(x => x.IsActiveInd == true) on tuj.UnitId equals u.Id
                   select new
                   {
                      u.UnitName,
                      u.Id
                   })
                   .OrderBy(x => x.UnitName)
                   .Distinct()
                  .ToListAsync().ConfigureAwait(false);

        public async Task<object> GetUnitsAndTractsByWell(long id) =>
          await (from w in context.Wells.Where(x => x.Id == id)
                 join tuwj in context.TractUnitJunctionWellJunctions on w.Id equals tuwj.WellId
                 join tuj in context.TractUnitJunctions.Include(x => x.Tract) on tuwj.TractUnitJunctionId equals tuj.Id
                 join u in context.Units.Where(x => x.IsActiveInd == true) on tuj.UnitId equals u.Id
                 select new
                 {
                     tuj.TractId,
                     tuj.Tract.TractNum,
                     COPAcresByUnit = tuj.COPAcres,
                     DPUAcresByUnit = u.DPUAcres,
                     u.UnitName,
                     u.AmendmentName,
                     u.Id
                 })
                 .OrderBy(x => x.UnitName)
                 .Distinct()
                .ToListAsync().ConfigureAwait(false);

        public async Task<object> GetNriTractInfobywell(long id)
        {
            var query = await (from w in context.Wells.Where(x => x.Id == id)
                               join tuwj in context.TractUnitJunctionWellJunctions on w.Id equals tuwj.WellId
                               join tuj in context.TractUnitJunctions.Include(x => x.Tract) on tuwj.TractUnitJunctionId equals tuj.Id
                               join u in context.Units.Where(x => x.IsActiveInd == true) on tuj.UnitId equals u.Id
                               select new
                               {
                                   tuj.TractId,
                                   u.DPUAcres,
                                   tuj.COPAcres,
                                   u.Id
                               })
                 .Distinct()
                .ToListAsync().ConfigureAwait(false);

            return query
                .GroupBy(l => l.TractId)
                                  .Select(cl => new
                                  {
                                      cl.First().TractId,
                                      COPAcres = cl.Sum(c => c.COPAcres),
                                      DPUAcres = cl.Sum(c => c.DPUAcres)
                                  }).ToList();

        }       

    }
    
}
