using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class UnitMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<Unit> genericRepository;
        private readonly IUnitRepository repository;
        private readonly IUnitBusinessLogic unitBusinessLogic;
        private readonly ICurrentUserRepository currentUserRepository;

        public UnitMgrApiController(
            IGenericRepository<Unit> genericRepo,
            IUnitRepository repo,
            IUnitBusinessLogic businessLogic,
            ICurrentUserRepository currentUserRepo)
        {
            genericRepository = genericRepo;
            repository = repo;
            unitBusinessLogic = businessLogic;
            currentUserRepository = currentUserRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok((await genericRepository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.UnitName));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericRepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("unitGroupInfoByUnit/{id}")]
        public async Task<IActionResult> GetUnitGroupInfoByUnit(long id) => Ok(await repository.GetUnitGroupInfoByUnit(id).ConfigureAwait(false));

        [HttpGet("unitsForDataTable")]
        public async Task<IActionResult> GetUnitsForDataTableAsync() => Ok(await repository.GetUnitsForDataTableAsync().ConfigureAwait(false));

        [HttpPost]
        public IActionResult Post([FromBody]  UnitViewModel model) =>
        Ok(unitBusinessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));

    }
}