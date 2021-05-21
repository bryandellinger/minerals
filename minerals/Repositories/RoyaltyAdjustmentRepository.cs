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
    public class RoyaltyAdjustmentRepository : IRoyaltyAdjustmentRepository
    {
        private readonly DataContext context;
        public RoyaltyAdjustmentRepository(DataContext ctx) => context = ctx;

        public void Delete(long id) 
        {
            RoyaltyAdjustment royaltyAdjustment = new RoyaltyAdjustment { Id = id };
            context.Remove(royaltyAdjustment);
            context.SaveChanges();
        }

        public async Task<object> GetByCheckAsync(long id) =>
        await context.RoyaltyAdjustments.Where(x => x.CheckId == id)
                 .Select(o => new
                 {
                     Id = o.RoyaltyId,
                     o.Royalty.WellTractInformation.Well.WellNum,
                     o.Royalty.WellTractInformation.Well.ApiNum,
                     o.Royalty.WellTractInformation.Lessee.LesseeName,
                     o.GasRoyalty,
                     o.Check.CheckDate,
                     o.Royalty.PaymentType.PaymentTypeName,
                     o.Royalty.WellTractInformation.Tract.TractNum,
                     o.Royalty.PostMonth,
                     o.Royalty.PostYear,
                     o.Royalty.LiqVolume,
                     o.Royalty.LiqPayment,
    })
            .ToListAsync().ConfigureAwait(false);

    public async Task<object> GetByPaymentAsync(long id) =>

            await context.RoyaltyAdjustments.Where(x => x.RoyaltyId == id)
            .OrderByDescending(x => x.EntryDate).ThenByDescending(x => x.Id)
            .Select(r => new
            {
                r.Id,
                r.NRI,
                r.OilProd,
                r.OilRoyalty,
                r.GasProd,
                r.GasRoyalty,
                r.Flaring,
                r.EntryDate,
                r.Deduction,
                r.CompressDeduction,
                r.TransDeduction,
                r.CheckId,
                r.Check.CheckNum,
                r.Check.CheckDate,
                r.Check.ReceivedDate,
                r.Check.TotalAmount,
                r.SalesPrice,
                r.LiqVolume,
                r.LiqPayment,
                r.LastUpdateDate,
            })
            .ToListAsync()
        .ConfigureAwait(false);

        public RoyaltyAdjustment Update(RoyaltyAdjustment model, long royaltyAdjustmentId, long userId)
        {
            var royaltyAdjustment = context.RoyaltyAdjustments.Find(royaltyAdjustmentId);
            royaltyAdjustment.OilProd = model.OilProd;
            royaltyAdjustment.GasProd = model.GasProd;
            royaltyAdjustment.LiqVolume = model.LiqVolume;
            royaltyAdjustment.OilRoyalty = model.OilRoyalty;
            royaltyAdjustment.GasRoyalty = model.GasRoyalty;
            royaltyAdjustment.LiqPayment = model.LiqPayment;
            royaltyAdjustment.NRI = model.NRI;
            royaltyAdjustment.Deduction = model.Deduction;
            royaltyAdjustment.CompressDeduction = model.CompressDeduction;
            royaltyAdjustment.TransDeduction = model.TransDeduction;
            royaltyAdjustment.SalesPrice = model.SalesPrice;
            royaltyAdjustment.CheckId = model.CheckId;
            royaltyAdjustment.Flaring = model.Flaring;
            royaltyAdjustment.UpdatedBy = userId;
            royaltyAdjustment.LastUpdateDate = DateTime.Now;
            context.SaveChanges();

            return context.RoyaltyAdjustments.Include(x => x.Royalty).Where(x => x.Id == royaltyAdjustmentId).First();
        }
    }
}
