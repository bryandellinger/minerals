using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Minerals.Contexts;
using Models;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoyaltyPaymentDataTableApiController : ControllerBase
    {
        private readonly DataContext context;
        private List<RoyaltyViewModel> royalties;

        public RoyaltyPaymentDataTableApiController(DataContext ctx)
        {
            context = ctx;
        }
       

        [HttpPost]
        public IActionResult Get()
        {

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var paymentTypeId = Request.Form["paymentTypeId"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                string to = Request.Form["to"].FirstOrDefault();
                string from = Request.Form["from"].FirstOrDefault();

                DateTime? toDate = string.IsNullOrEmpty(to) ? (DateTime?)null : DateTime.ParseExact(to, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime? fromDate = string.IsNullOrEmpty(from) ? (DateTime?)null : DateTime.ParseExact(from, "MM/dd/yyyy", CultureInfo.InvariantCulture);


                royalties = context.Royalties
                .Where(x => x.PaymentTypeId == long.Parse(paymentTypeId))
                .Where(x => x.Check.CheckDate != null)
                .Where(x => fromDate == null   || x.EntryDate.Value.Date >= fromDate.Value)
                .Where(x => toDate == null  || x.EntryDate.Value.Date <= toDate.Value)
                 .Select(o => new RoyaltyViewModel
                 {
                     Id = o.Id,
                     EntryDate = o.EntryDate,
                     CheckNum = o.Check.CheckNum,
                     LesseeName = o.WellTractInformation.LesseeName,
                     WellNum = o.WellTractInformation.Well.WellNum,
                     WellId = o.WellTractInformation.WellId,
                     ApiNum = o.WellTractInformation.Well.ApiNum,
                     GasRoyalty = o.LiqPayment != null ? o.LiqPayment : o.GasRoyalty,
                     CheckDate = o.Check.CheckDate,
                     PaymentTypeName = o.PaymentType.PaymentTypeName,
                     TractNum = o.WellTractInformation.Tract.TractNum,
                     PostMonth = o.PostYear != null && o.PostMonth != null ? new DateTime(o.PostYear.Value, o.PostMonth.Value, 1, 0, 0, 0) : (DateTime?)null
                 })
            .ToList();

                if (string.IsNullOrEmpty(sortColumn) || sortColumn == "id")
                {
                    sortColumn = "entryDate";
                    sortColumnDirection = "desc";
                }

                    var orderedRoyalties = royalties.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                    royalties = orderedRoyalties.ToList();
              
               
                if (!string.IsNullOrEmpty(searchValue))
                {
                    royalties = royalties.
                        Where(m =>  (m.CheckNum??string.Empty).Contains(searchValue) ||
                                    (m.ApiNum??string.Empty).ToLower().Contains(searchValue.ToLower()) ||
                                    (m.LesseeName ?? string.Empty).ToLower().Contains(searchValue.ToLower()) ||
                                    (m.WellNum ?? string.Empty).ToLower().Contains(searchValue.ToLower()) ||
                                     (m.PaymentTypeName ?? string.Empty).ToLower().Contains(searchValue.ToLower()) ||
                                    (m.GasRoyalty.ToString() ?? string.Empty).Contains(searchValue.ToLower()) ||
                                     (m.TractNum ?? string.Empty).ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
                                              
                }
                recordsTotal = royalties.Count;
                var data = royalties
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                foreach (var item in data)
                {
                    if (item.WellId != null)
                    {
                        item.UnitNames = GetUnits(item.WellId);
                    }
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception )
            {
                throw;
            }
        }

        private string GetUnits(long? wellId)
        {
            var result = (from tractUnitJunctionWellJunctions in context.TractUnitJunctionWellJunctions.Where(x => x.WellId == wellId.Value)
                          join tractUnitJunctions in context.TractUnitJunctions on tractUnitJunctionWellJunctions.TractUnitJunctionId equals tractUnitJunctions.Id
                          join units in context.Units on tractUnitJunctions.UnitId equals units.Id
                          select units.UnitName)
                             .OrderBy(x => x)
                             .Distinct().ToArray();

            var unitNames = String.Join(",", result);
            return unitNames;

        }

        private class RoyaltyViewModel
        {

            public long Id { get; set; }
            public string CheckNum { get; set; }
            public string WellNum { get; set; }
            public long? WellId { get; set; }
            public string ApiNum { get; set; }
            public decimal? GasRoyalty { get;  set; }
            public DateTime? CheckDate { get; set; }
            public DateTime? EntryDate { get; set; }
            public string LesseeName { get; set; }
            public string PaymentTypeName { get; set; }
            public string TractNum { get; set; }
            public DateTime? PostMonth { get; set; }
            public string UnitNames { get; set; }
        }
    }
}