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
    public class PluggingSuretyDetailRepository : IPluggingSuretyDetailRepository
    {
        private readonly DataContext context;
        public PluggingSuretyDetailRepository(DataContext ctx) => context = ctx;
        public async Task<IEnumerable<PluggingSuretyDetail>> GetPluggingSuretyDetailsByContractAsync(long Id) =>
        
            await Task.FromResult((from contract in context.Contracts.Where(x => x.Id == Id)
                                   join paymentRequirement in context.PaymentRequirements on contract.PaymentRequirementId equals paymentRequirement.Id
                                   join pluggingSuretyDetail in context.PluggingSuretyDetails on paymentRequirement.Id equals pluggingSuretyDetail.PaymentRequirementId
                                   select new PluggingSuretyDetail
                                   {
                                       Id = 0,
                                       FromDepth = pluggingSuretyDetail.FromDepth,
                                       ToDepth = pluggingSuretyDetail.ToDepth,
                                       MinimumBondAmount = pluggingSuretyDetail.MinimumBondAmount,
                                       MeasurementType = pluggingSuretyDetail.MeasurementType
                                   })
                         .ToList()).ConfigureAwait(false);
        
    }
}

