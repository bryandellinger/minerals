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
    public class WellBoreShareMgrApiController : ControllerBase
    {
        private IWellBoreShareRepository repository;
        public WellBoreShareMgrApiController(IWellBoreShareRepository repo) => repository = repo;

        [HttpGet("wellboresharessbywell/{id}")]
        public async Task<IActionResult> Get(long id) => Ok((await repository.GetWellBoreSharessByWellAsync(id).ConfigureAwait(false)));

    }
}