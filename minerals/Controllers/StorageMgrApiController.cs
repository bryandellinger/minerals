using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]

    public class StorageMgrApiController : ControllerBase
    {
        private readonly IStorageRepository repository;
        public StorageMgrApiController(IStorageRepository repo) => repository = repo;

        [HttpGet("getByContract/{id}")]
        public async Task<IActionResult> GetByContract(long id) => Ok((await repository.GetByContractAsync(id).ConfigureAwait(false)));

    }
}