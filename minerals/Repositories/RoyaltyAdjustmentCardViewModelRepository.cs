using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Minerals.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class RoyaltyAdjustmentCardViewModelRepository : IRoyaltyAdjustmentCardViewModelRepository
    {
        private readonly CultureInfo ci;
        private readonly DataContext context;

        public RoyaltyAdjustmentCardViewModelRepository(DataContext ctx)
        {
            context = ctx;
            ci = new CultureInfo("en-US");
        }

        public Task<IEnumerable<RoyaltyAdjustmentCardViewModel>> Get(string search) =>
            Task.FromResult<IEnumerable<RoyaltyAdjustmentCardViewModel>>(context.RoyaltyAdjustmentCardViewModels.FromSqlRaw(
                 @"select 
                    CONVERT(varchar, r.Id) as Id, l.LesseeName, w.WellNum, rr.CheckNum, t.TractNum, p.PadName,
                    case r.PostMonth when 1 then 'Jan' when 2 then 'Feb'when 3 then 'March' when 4 then 'April' when 5 then 'May' when 6 then 'Jun'
                    when 7 then 'July' when  8 then 'Aug' when 9 then 'Sep' when 10 then 'Oct' when 11 then 'Nov'  when 12 then 'Dec' else Null End as PostMonth,
                    CONVERT(varchar, r.PostYear) PostYear,
                    Convert(varchar, rr.CheckDate,110) CheckDate,
                    Convert(varchar, rr.RecvDate,110) RecvDate,
                    Convert(varchar, Convert(DOUBLE PRECISION,  r.GasProd)) GasProd,
                    Convert(varchar, Convert(DOUBLE PRECISION,  r.OilProd)) OilProd,
                    Convert(varchar, Convert(DOUBLE PRECISION,  r.GasRoyalty)) GasRoyalty,
                    Convert(varchar, Convert(DOUBLE PRECISION,  r.OilRoyalty)) OilRoyalty
                  from
                        royalties r join WellOperations wo on r.WellOperationId = wo.Id
                        join Wells w on wo.WellId = w.Id 
                        join Pads p on w.PadId = p.Id
                        join Tracts t on p.TractId = t.Id
                        join RevenueReceived rr on r.RevenueReceivedId = rr.Id
                        join Lessees l on rr.LesseeId = l.Id"
                ).
                Where(
                 x => search == null ||
                 (
                   x.Id == search ||
                   x.LesseeName.ToLower(new CultureInfo("en-US", false)).StartsWith(search.ToLower(ci), false, ci) ||
                   x.WellNum.ToLower(new CultureInfo("en-US", false)) == search.ToLower(ci) ||
                   x.CheckNum.ToLower(new CultureInfo("en-US", false)).StartsWith(search.ToLower(ci), false, ci) ||
                   x.TractNum.ToLower(new CultureInfo("en-US", false)) == search.ToLower(ci) ||
                   x.PadName.ToLower(new CultureInfo("en-US", false)) == search.ToLower(ci) ||
                   x.PostMonth.ToLower(new CultureInfo("en-US", false)) == search.ToLower(ci) ||
                   x.CheckDate == search ||
                   x.RecvDate == search ||
                   x.PostYear == search ||
                   x.GasProd == search ||
                   x.OilProd == search ||
                   x.GasRoyalty == search ||
                   x.OilRoyalty == search
                 )
                )
                .ToArray()
                .OrderByDescending(x => int.Parse(x.Id, new CultureInfo("en-US")))
                .Take(30000));

        public Task<IEnumerable<RoyaltyAdjustmentCardViewModel>> Search(RoyaltyAdjustmentCardViewModel searchCriteria) =>
        Task.FromResult<IEnumerable<RoyaltyAdjustmentCardViewModel>>(context.RoyaltyAdjustmentCardViewModels.FromSqlRaw(
                @"select 
                    CONVERT(varchar, r.Id) as Id, l.LesseeName, w.WellNum, rr.CheckNum, t.TractNum, p.PadName,
                    case r.PostMonth when 1 then 'Jan' when 2 then 'Feb'when 3 then 'March' when 4 then 'April' when 5 then 'May' when 6 then 'Jun'
                    when 7 then 'July' when  8 then 'Aug' when 9 then 'Sep' when 10 then 'Oct' when 11 then 'Nov'  when 12 then 'Dec' else Null End as PostMonth,
                    CONVERT(varchar, r.PostYear) PostYear,
                    Convert(varchar, rr.CheckDate,110) CheckDate,
                    Convert(varchar, rr.RecvDate,110) RecvDate,
                    Convert(varchar, Convert(DOUBLE PRECISION,  r.GasProd)) GasProd,
                    Convert(varchar, Convert(DOUBLE PRECISION,  r.OilProd)) OilProd,
                    Convert(varchar, Convert(DOUBLE PRECISION,  r.GasRoyalty)) GasRoyalty,
                    Convert(varchar, Convert(DOUBLE PRECISION,  r.OilRoyalty)) OilRoyalty
                  from
                        royalties r join WellOperations wo on r.WellOperationId = wo.Id
                        join Wells w on wo.WellId = w.Id 
                        join Pads p on w.PadId = p.Id
                        join Tracts t on p.TractId = t.Id
                        join RevenueReceived rr on r.RevenueReceivedId = rr.Id
                        join Lessees l on rr.LesseeId = l.Id"
               ).
               Where(
                x =>
                  (searchCriteria.LesseeName == null || string.IsNullOrEmpty(searchCriteria.LesseeName) || x.LesseeName.ToLower(ci).StartsWith(searchCriteria.LesseeName.ToLower(ci),false,ci)) &&
                  (searchCriteria.CheckNum == null || string.IsNullOrEmpty(searchCriteria.CheckNum) || string.IsNullOrWhiteSpace(searchCriteria.CheckNum) || x.CheckNum == searchCriteria.CheckNum) &&
                  (searchCriteria.TractNum == null || string.IsNullOrEmpty(searchCriteria.TractNum) || string.IsNullOrWhiteSpace(searchCriteria.TractNum) || x.TractNum.ToLower(ci) == searchCriteria.TractNum.ToLower(ci)) &&
                  (searchCriteria.PadName == null || string.IsNullOrEmpty(searchCriteria.PadName) || string.IsNullOrWhiteSpace(searchCriteria.PadName) || x.PadName.ToLower(ci).StartsWith(searchCriteria.PadName.ToLower(ci),false,ci)) &&
                  (searchCriteria.WellNum == null || string.IsNullOrEmpty(searchCriteria.WellNum) || string.IsNullOrWhiteSpace(searchCriteria.WellNum) || x.WellNum.ToLower(ci).StartsWith(searchCriteria.WellNum.ToLower(ci),false,ci))
               )
               .ToArray()
               .OrderByDescending(x => int.Parse(x.Id, new CultureInfo("en-US")))
               .Take(!searchCriteria.GetType().GetProperties().Any(prop => prop == null) ? 1000 : 30000));

    }
}
