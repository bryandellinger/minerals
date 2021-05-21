using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class RowContractRepository : IRowContractRepository
    {
        private readonly DataContext context;
        public RowContractRepository(DataContext ctx) => context = ctx;
        public async Task<IEnumerable<RowContract>> GetAllRowContractsByContractAsync(long id)  => await context.RowContracts.Where(x => x.ContractId == id).ToListAsync().ConfigureAwait(false);
    }
}
