using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class AssociatedTractRepository : IAssociatedTractRepository
    {
        private readonly DataContext context;
        public AssociatedTractRepository(DataContext ctx) => context = ctx;

        public async Task<IEnumerable<AssociatedTract>> GetAllAssociatedTractsByContractAsync(long id) => 
            await context.AssociatedTracts.Where(x => x.ContractId == id).ToListAsync().ConfigureAwait(false);
    }
}
