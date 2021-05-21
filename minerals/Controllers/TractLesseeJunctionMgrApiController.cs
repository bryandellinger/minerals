using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class TractLesseeJunctionMgrApiController : ControllerBase
    {
        private readonly ITractLesseeJunctionRepository repository;

        public TractLesseeJunctionMgrApiController(ITractLesseeJunctionRepository repo) => repository = repo;

        /// <summary> given a lessee id get all tracts as JSON</summary>
        [HttpGet("tractsbylessee/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetAllTractsByLesseeAsync(id).ConfigureAwait(false));
    }
}