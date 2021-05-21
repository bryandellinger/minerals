using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RevenueReceivedMgrApiController : ControllerBase
    {
        private readonly IRevenueReceivedRepository repository;
        public RevenueReceivedMgrApiController(IRevenueReceivedRepository repo) => repository = repo;

        /// <summary> Get all Revenue Received as JSON</summary>
        [HttpGet]
        [Authorize]
        [AuthorizeUser(Roles = "read")]
        public async Task<IActionResult> Get() => Ok(await repository.GetAllAsync().ConfigureAwait(false));

    }
}