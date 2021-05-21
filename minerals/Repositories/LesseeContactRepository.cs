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
    public class LesseeContactRepository : ILesseeContactRepository
    {
        private readonly DataContext context;
        public LesseeContactRepository(DataContext ctx) => context = ctx;
        public async Task<IEnumerable<LesseeContact>> GetLesseeContactsByLesseeAsync(long Id) =>
            await context.LesseeContacts.Where(x => x.LesseeId == Id).ToListAsync().ConfigureAwait(false);

    }
}
