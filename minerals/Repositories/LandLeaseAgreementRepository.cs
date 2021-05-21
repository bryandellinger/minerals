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
    public class LandLeaseAgreementRepository : ILandLeaseAgreementRepository
    {
        private readonly DataContext context;
        public LandLeaseAgreementRepository(DataContext ctx) => context = ctx;
        public  async Task<LandLeaseAgreement> GetLandLeaseAgreementByContractAsync(long Id) => await context.LandLeaseAgreements.FirstOrDefaultAsync(x => x.ContractId == Id).ConfigureAwait(false);

    }
}
