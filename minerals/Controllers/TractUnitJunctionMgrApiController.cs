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
    public class TractUnitJunctionMgrApiController : ControllerBase
    {
        private readonly ITractUnitJunctionRepository repository;

        public TractUnitJunctionMgrApiController(ITractUnitJunctionRepository repo) => repository = repo;

        [HttpGet("junctionsbyunit/{id}")]
        public async Task<IActionResult> GetJunctionsByUnit(long id) => Ok(await repository.GetJunctionsByUnitAsync(id).ConfigureAwait(false));

        [HttpGet("junctionbytract/{id}")]
        public async Task<IActionResult> GetJunctionByTract(long  id) => Ok(await repository.GetJunctionByTractAsync(id).ConfigureAwait(false));
    }
}