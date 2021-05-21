using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class ApiCodeRepository : IApiCodeRepository
    {
        private readonly DataContext context;

        public ApiCodeRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetCountyCodes(long id) =>

            await context.ApiCodes
                .Where(x => x.StateCode == id)
                .OrderBy(x => x.CountyName)
                .Select(x => new { x.CountyCode, x.CountyName })
                .ToListAsync()
                .ConfigureAwait(false);
        

        public async Task<object> GetStateCodes() =>
            await context.ApiCodes
                .OrderBy(x => x.StateName)
                .Select(x => new {x.StateCode, x.StateName })
                .Distinct()
                .ToListAsync()
                .ConfigureAwait(false);

    }
}
