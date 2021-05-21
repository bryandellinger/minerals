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
    public class SuretyMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<Surety> genericrepository;
        private readonly ICurrentUserRepository currentUserRepository;
        private readonly ISuretyBusinessLogic businessLogic;
        private readonly ISuretyRepository repository;
        private readonly IGenericRepository<SuretyRider> suretyRiderRepository;
        private readonly IGenericRepository<SuretyWell> suretyWellRepository;
        private readonly IGenericRepository<RiderReason> riderReasonRepository;

        public SuretyMgrApiController(
             IGenericRepository<Surety> genericrepo,
             ICurrentUserRepository currentUserRepo,
             ISuretyBusinessLogic business,
             ISuretyRepository repo,
             IGenericRepository<SuretyRider> suretyRiderRepo,
             IGenericRepository<SuretyWell> suretyWellRepo,
             IGenericRepository<RiderReason> riderReasonRepo
            )
           {
            genericrepository = genericrepo;
            currentUserRepository = currentUserRepo;
            businessLogic = business;
            repository = repo;
            suretyRiderRepository = suretyRiderRepo;
            suretyWellRepository = suretyWellRepo;
            riderReasonRepository = riderReasonRepo;
           }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await repository.GetAsync().ConfigureAwait(false));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericrepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("GetRiderReasons")]
        public async Task<IActionResult> GetSuretyRiderReasonsAsync() =>
                Ok((await riderReasonRepository.GetAllAsync().ConfigureAwait(false)) .OrderBy(x => x.RiderReasonName));

        [HttpGet("GetSuretyRidersBySurety/{id}")]
        public async Task<IActionResult> GetSuretyRidersBySuretyAsync(long id) =>
              Ok((await suretyRiderRepository.GetAllAsync().ConfigureAwait(false))
                  .Where(x => x.SuretyId == id)
                  .OrderBy(x => x.EffectiveDate)
                  .ToList());

        [HttpGet("GetSuretyWellsBySurety/{id}")]
        public async Task<IActionResult> GetSuretyWellsBySuretyAsync(long id) =>
         Ok((await suretyWellRepository.GetAllAsync().ConfigureAwait(false))
             .Where(x => x.SuretyId == id)
             .ToList());

        [HttpGet("GetSuretiesByWell/{id}")]
        public async Task<IActionResult> GetSuretiesByWellAsync(long id) =>
            Ok(await repository.GetSuretiesByWellAsync(id).ConfigureAwait(false));

        [HttpGet("GetSuretiesByContract/{id}")]
        public async Task<IActionResult> GetSuretiesByContractAsync(long id) =>
          Ok(await repository.GetSuretiesByContractAsync(id).ConfigureAwait(false));

        [HttpGet("GetInsurers")]
        public async Task<IActionResult> GetInsurers() =>
         Ok((await genericrepository.GetAllAsync().ConfigureAwait(false))
             .Where(x => !string.IsNullOrEmpty(x.Insurer))
             .OrderBy(x => x.Insurer)
             .Select(x => x.Insurer)
             .Distinct()
             .ToList()
             );

        [HttpPost]
        public IActionResult Post([FromBody]  Surety model) =>
            Ok(businessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));

    }
}