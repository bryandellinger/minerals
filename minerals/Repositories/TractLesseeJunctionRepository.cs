using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class TractLesseeJunctionRepository : ITractLesseeJunctionRepository
    {
        private readonly DataContext context;

        public TractLesseeJunctionRepository(DataContext ctx) => context = ctx;
        public async Task<IEnumerable<Tract>> GetAllTractsByLesseeAsync(long Id) =>
            await   context.TractLesseeJunctions.Where(x => x.LesseeId == Id).Include(x => x.Tract).Select(x => x.Tract).ToListAsync().ConfigureAwait(false);

    }
}
