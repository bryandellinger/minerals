using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Minerals.Repositories
{
    public class PaymentRequirementRepository : IPaymentRequirementRepository
    {
        private readonly DataContext context;
        public PaymentRequirementRepository(DataContext ctx) => context = ctx;
        public async Task<PaymentRequirement> GetPaymentRequirementByContractAsync(long Id) =>
            await context.PaymentRequirements.FirstOrDefaultAsync(
                p => context.Contracts.Any(c => c.Id == Id && c.PaymentRequirement == p)
                ).ConfigureAwait(false);

        public async Task<object> GetPaymentRequirementsByContractAsync(long[] ids) { 

       var retval =  await context.PaymentRequirements.Where(
             p => context.Contracts.Any(c => ids.Contains(c.Id) && c.PaymentRequirement == p)
                ).ToListAsync().ConfigureAwait(false);
        return retval;
    }

    }
}
