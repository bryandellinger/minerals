using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Models;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class ContractTypeMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<ContractType> repository;

        public ContractTypeMgrApiController(IGenericRepository<ContractType> repo) => repository = repo;

        /// <summary> Returns all Contract Types as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok(await repository.GetAllAsync().ConfigureAwait(false));

    }
}