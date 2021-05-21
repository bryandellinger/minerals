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
    public class LesseeHistoryRepository : ILesseeHistoryRepository
    {
        private readonly DataContext context;
        public LesseeHistoryRepository(DataContext ctx) => context = ctx;

        public async Task<IEnumerable<LesseeHistory>> GetLesseeHistoryByLesseeAsync(long Id) =>
            await context.LesseeHistories.Where(x => x.LesseeId == Id).ToListAsync().ConfigureAwait(false);
    }
}
