using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class AdditionalBonusRepository : IAdditionalBonusRepository
    {
        private readonly DataContext context;
        public AdditionalBonusRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetByContractAsync(long id) =>
      await (from contracts in context.Contracts.Where(x => x.Id == id)
             join paymentRequirements in context.PaymentRequirements
                on contracts.PaymentRequirementId equals paymentRequirements.Id
             join additionalBonuses in context.AdditionalBonuses
                 on paymentRequirements.Id equals additionalBonuses.PaymentRequirementId
             select new AdditionalBonus
             {
                 Id = additionalBonuses.Id,
                 AdditionalBonusAmount = additionalBonuses.AdditionalBonusAmount,
                 PaymentRequirementId = additionalBonuses.PaymentRequirementId
             })
            .ToListAsync()
            .ConfigureAwait(false);              
    }
}
