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
    public class SurfaceUseAgreementRepository : ISurfaceUseAgreementRepository
    {
        private readonly DataContext context;
        public SurfaceUseAgreementRepository(DataContext ctx) => context = ctx;
        public async Task<SurfaceUseAgreement> GetSurfaceUseAgreementByContractAsync(long Id) => await context.SurfaceUseAgreements.FirstOrDefaultAsync(x => x.ContractId == Id).ConfigureAwait(false);
    }
}
