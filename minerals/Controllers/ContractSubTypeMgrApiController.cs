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
    public class ContractSubTypeMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<ContractSubType> repository;
        public ContractSubTypeMgrApiController(IGenericRepository<ContractSubType> repo) => repository = repo;

        /// <summary> Returns all Contract  as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok(await repository.GetAllAsync().ConfigureAwait(false));

    }
}