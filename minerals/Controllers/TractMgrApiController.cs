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
    public class TractMgrApiController : ControllerBase
    {
        private readonly ITractRepository tractRepository;
        private readonly ITractBusinessLogic businessLogic;

        public TractMgrApiController(ITractRepository tractRepo, ITractBusinessLogic tractBusinessLogic)
        {
            tractRepository = tractRepo;
            businessLogic = tractBusinessLogic;
        }

        /// <summary>get all tracts as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok((await tractRepository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.TractNum));

        /// <summary>get a tracts as JSON given a tract id</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await tractRepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpPost]
        public IActionResult Post([FromBody]  Tract model)
        {
            var tract = businessLogic.Save(model);
            return Ok(new Tract {Id = tract.Id });
        }
    }
}