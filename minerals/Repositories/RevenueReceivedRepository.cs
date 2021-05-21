using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class RevenueReceivedRepository : IRevenueReceivedRepository
    {
        private readonly DataContext context;
        public RevenueReceivedRepository(DataContext ctx) => context = ctx;

        public async Task<IEnumerable<RevenueReceived>> GetAllAsync() => await context.RevenueReceived.Include(x => x.Lessee).OrderByDescending(x => x.CheckDate).ToListAsync().ConfigureAwait(false);

    }
}
