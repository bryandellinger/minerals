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
    public class PadMgrApiController : ControllerBase
    {
        private readonly IPadRepository repository;
        private readonly IGenericRepository<Pad> genericRepository;

        public PadMgrApiController(IPadRepository repo, IGenericRepository<Pad> genericRepo)
        {
            repository = repo;
            genericRepository = genericRepo;
        }
        /// <summary> Given a tract id get all pads as JSON</summary>
        [HttpGet("padsbytract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetAllPadsByTractAsync(id).ConfigureAwait(false));

        /// <summary> Return all pads as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok((await genericRepository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.PadName));

    }
}