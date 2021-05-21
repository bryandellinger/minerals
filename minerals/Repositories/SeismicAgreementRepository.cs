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
    public class SeismicAgreementRepository : ISeismicAgreementRepository
    {
        private readonly DataContext context;
        public SeismicAgreementRepository(DataContext ctx) => context = ctx;
        public async Task<SeismicAgreement> GetSeismicAgreementByContractAsync(long Id) => await context.SeismicAgreements.FirstOrDefaultAsync(x => x.ContractId == Id).ConfigureAwait(false);
    }
}
