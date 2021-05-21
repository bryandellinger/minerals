using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class RoyaltyRepository : IRoyaltyRepository
    {
        private readonly DataContext context;

        public RoyaltyRepository(DataContext ctx) => context = ctx;

        public Royalty CheckProdMonth(long lesseeId, int postMonth, int postYear, long paymentTypeId, long wellId) =>
      (from wellTractInformation in context.WellTractInformations.Where(x => x.LesseeId == lesseeId && x.WellId == wellId && x.ActiveInd)
       join royalty in context.Royalties
       .Where(x => x.PostMonth.Value == postMonth && x.PostYear.Value == postYear && x.PaymentTypeId == paymentTypeId)
       on wellTractInformation.Id equals royalty.WellTractInformationId
       select new Royalty { Id = royalty.Id }).FirstOrDefault();


        public async Task<object> CheckProdMonthAsync(long lesseeId, int postMonth, int postYear, long paymentTypeId, long wellId)
        {
            var x = await (from wellTractInformation in context.WellTractInformations.Where(x => x.WellId == wellId && x.ActiveInd)
                           join royalty in context.Royalties
                           .Where(x => x.PostMonth.Value == postMonth && x.PostYear.Value == postYear && x.PaymentTypeId == paymentTypeId)
                           on wellTractInformation.Id equals royalty.WellTractInformationId
                           join check in context.Checks.Where(x => x.LesseeId == lesseeId) on royalty.CheckId equals check.Id
                           select new Royalty { Id = royalty.Id })
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return x;
        }


        public async Task<object> GetByCheck(long id) =>
             await context.Royalties.Where(x => x.CheckId == id)
                 .Select(o => new
                 {
                     o.Id,
                     o.WellTractInformation.Well.WellNum,
                     o.WellTractInformation.Well.ApiNum,
                     o.WellTractInformation.Lessee.LesseeName,
                     o.GasRoyalty,
                     o.Check.CheckDate,
                     o.PaymentType.PaymentTypeName,
                     o.WellTractInformation.Tract.TractNum,
                     o.PostMonth,
                     o.PostYear
                 })
            .ToListAsync().ConfigureAwait(false);

        public async Task<object> GetCheckByIdAsync(long id) =>
        await context.Royalties
               .Where(x => x.Id == id)
               .Select(o => new
               {
                   o.Check.Id,
                   o.Check.CheckDate,
                   o.Check.ReceivedDate,
                   o.Check.LesseeId,
                   o.Check.LesseeName,
               })
               .FirstOrDefaultAsync()
               .ConfigureAwait(false);

        public async Task<Royalty> Insert(Royalty royalty)
        {
            this.context.Royalties.Add(royalty);
            await this.context.SaveChangesAsync().ConfigureAwait(false);
            return royalty;
        }

        public void Update(RoyaltyViewModel model, long userId)
        {
            var royalty = context.Royalties.Find(model.Id);         
            if (! model.AdjustmentInd)
            {
                royalty.CheckId = model.CheckId;
                royalty.CheckNum = model.CheckNum;
            }
            royalty.WellTractInformationId = model.WellTractInformationId;
            royalty.PaymentTypeId = model.PaymentTypeId;
            royalty.EntryDate = model.EntryDate;
            royalty.PostMonth = model.PostMonth;
            royalty.PostYear = model.PostYear;
            royalty.NRI = model.NRI;
            royalty.GasProd = model.GasProd;
            royalty.OilProd = model.OilProd;
            royalty.GasRoyalty = model.GasRoyalty;
            royalty.OilRoyalty = model.OilRoyalty;
            royalty.SalesPrice = model.SalesPrice;
            royalty.Deduction = model.Deduction;
            royalty.LiqVolume = model.LiqVolume;
            royalty.LiqPayment = model.LiqPayment;
            royalty.LiqMeasurement = model.LiqMeasurement;
            royalty.ProductTypeId = model.ProductTypeId;
            royalty.TransDeduction = model.TransDeduction;
            royalty.CompressDeduction = model.CompressDeduction;
            royalty.Flaring = model.Flaring;
            royalty.RoyaltyNotes = model.RoyaltyNotes;
            royalty.LastUpdateDate = DateTime.Now;
            royalty.UpdatedBy = userId;
            context.SaveChanges();
        }
    }
}
