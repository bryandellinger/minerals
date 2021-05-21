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
    public class ProspectingAgreementRepository : IProspectingAgreementRepository
    {
        private readonly DataContext context;
        public ProspectingAgreementRepository(DataContext ctx) => context = ctx;
        public async Task<ProspectingAgreement> GetProspectingAgreementByContractAsync(long Id) => await context.ProspectingAgreements.FirstOrDefaultAsync(x => x.ContractId == Id).ConfigureAwait(false);
    }
}
