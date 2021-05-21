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
    public class ProductionAgreementRepository : IProductionAgreementRepository
    {
        private readonly DataContext context;
        public ProductionAgreementRepository(DataContext ctx) => context = ctx;
        public async Task<ProductionAgreement> GetProductionAgreementByContractAsync(long Id) =>
            await context.ProductionAgreements.FirstOrDefaultAsync(x => x.ContractId == Id).ConfigureAwait(false);
    }
}
