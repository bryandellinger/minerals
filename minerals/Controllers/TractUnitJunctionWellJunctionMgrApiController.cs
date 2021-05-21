using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class TractUnitJunctionWellJunctionMgrApiController : ControllerBase
    {
        private ITractUnitJunctionWellJunctionRepository repository;
        public TractUnitJunctionWellJunctionMgrApiController(ITractUnitJunctionWellJunctionRepository repo) => repository = repo;

        [HttpGet("unitsbywell/{id}")]
        public async Task<IActionResult> GetUnitsByWell(long id) => Ok(await repository.GetUnitsByWell(id).ConfigureAwait(false));

        [HttpGet("unitsandtractsbywell/{id}")]
        public async Task<IActionResult> GetUnitsAndTractsByWell(long id) => Ok(await repository.GetUnitsAndTractsByWell(id).ConfigureAwait(false));

        [HttpGet("nriTractInfobywell/{id}")]
        public async Task<IActionResult> GetNriTractInfobywelll(long id) => Ok(await repository.GetNriTractInfobywell(id).ConfigureAwait(false));
    }
}