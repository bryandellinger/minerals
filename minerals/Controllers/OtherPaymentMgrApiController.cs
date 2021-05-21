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
    public class OtherPaymentMgrApiController : ControllerBase
    {
        private readonly IOtherPaymentRepository repository;
        private readonly IGenericRepository<OtherRental> genericRepository;
        private readonly IOtherPaymentBusinessLogic businessLogic;
        private readonly ICurrentUserRepository currentUserRepository;

        public OtherPaymentMgrApiController(
            IOtherPaymentRepository repo,
            IGenericRepository<OtherRental> genericRepo,
            IOtherPaymentBusinessLogic contractRentalBusinessLogic,
           ICurrentUserRepository currentUserRepo
            )
        {
            repository = repo;
            genericRepository = genericRepo;
            businessLogic = contractRentalBusinessLogic;
            currentUserRepository = currentUserRepo;
        }

        [HttpGet()]
        public async Task<IActionResult> Get() => Ok((await repository.GetAllAsync().ConfigureAwait(false)));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericRepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("getByCheck/{id}")]
        public async Task<IActionResult> GetByCheck(long id) => Ok(await repository.GetByCheckAsync(id).ConfigureAwait(false));

        [HttpGet("getOtherPaymentTypes")]
        public async Task<IActionResult> GetOtherPaymentTypes() =>
            Ok((await genericRepository.GetAllAsync().ConfigureAwait(false))
                .Where(x => !string.IsNullOrEmpty(x.OtherPaymentType))
                .OrderBy(x => x.OtherPaymentType)
                .Select(x => x.OtherPaymentType)
                .Distinct()
                .ToList()
                );

        [HttpPost]
        public IActionResult Post([FromBody]  OtherRental model) =>
     Ok(businessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));
    }
}