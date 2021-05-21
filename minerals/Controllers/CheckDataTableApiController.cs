using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Minerals.Contexts;
using Models;
using System.Linq.Dynamic.Core;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckDataTableApiController : ControllerBase
    {
        private readonly DataContext context;
        private List<Check> checks;

        public CheckDataTableApiController(DataContext ctx) => context = ctx;

        [HttpPost]
        public IActionResult Get()
        {

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (string.IsNullOrEmpty(sortColumn) || sortColumn == "id")
                {
                    sortColumn = "receivedDate";
                    sortColumnDirection = "desc";
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    var orderedChecks = context.Checks.OrderBy(sortColumn + " " + sortColumnDirection);
                    checks= orderedChecks.ToList();
                }
                else
                {
                    checks = context.Checks.OrderByDescending(x => x.ReceivedDate).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    checks = checks.
                        Where(m => m.CheckNum.Contains(searchValue)
                                    || m.LesseeName.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
                                              
                }
                recordsTotal = checks.Count;
                var data = checks.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception )
            {
                throw;
            }
        }

    }
}