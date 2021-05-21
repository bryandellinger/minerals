using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class ContractEventDetailRepository : IContractEventDetailRepository
    {
        private readonly DataContext context;
        public ContractEventDetailRepository(DataContext ctx) => context = ctx;

        public void DeleteAll(List<ContractEventDetail> currentEventDetailsToRemove)
        {
            context.ContractEventDetails.RemoveRange(currentEventDetailsToRemove);
            context.SaveChanges();
        }

        public async Task<IEnumerable<ContractEventDetail>> GetContractEventDetailsByContractAsync(long id) =>
            await context.ContractEventDetails.Where(x => x.ContractId == id).ToListAsync().ConfigureAwait(false);

        public void InsertAll(List<ContractEventDetail> contractEventDetails)
        {
            context.ContractEventDetails.AddRange(contractEventDetails);
            context.SaveChanges();
        }

        public void UpdateCurrentContractEventDetailLeseeNamesByLesseeId(long lesseeId, string lesseeName)
        {
            foreach (var item in context.ContractEventDetails.Where(x => x.LesseeId == lesseeId && x.ActiveInd))
            {
                item.LesseeName = lesseeName;
            }
            context.SaveChanges();
        }
    }
}
