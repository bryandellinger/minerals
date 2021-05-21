using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class PadRepository : IPadRepository
    {
        private readonly DataContext context;

        public PadRepository(DataContext ctx) => context = ctx;

        public async Task<IEnumerable<Pad>> GetAllPadsByTractAsync(long Id) => await context.Pads.Where(x => x.TractId == Id).ToListAsync().ConfigureAwait(false);

    }
}
