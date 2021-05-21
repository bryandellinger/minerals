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
    public class CheckMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<Check> genericrepository;
        private readonly ICurrentUserRepository currentUserRepository;
        private readonly ICheckBusinessLogic checkBusinessLogic;
        private readonly IRoyaltyRepository royaltyrepository;
        private readonly ICheckRepository repository;


        public CheckMgrApiController(
            IGenericRepository<Check> genericrepo,
            ICurrentUserRepository currentUserRepo,
            ICheckBusinessLogic businessLogic,
            ICheckRepository repo,
            IRoyaltyRepository royaltyrepo
            )
        {
            genericrepository = genericrepo;
            currentUserRepository = currentUserRepo;
            checkBusinessLogic = businessLogic;
            repository = repo;
            royaltyrepository = royaltyrepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericrepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("GetSelect2Data")]
        public async Task<IActionResult> GetSelect2Data(string Id) => Ok(await repository.GetSelect2Data(Id).ConfigureAwait(false));

        [HttpGet("GetCheckByCheckNum/{id}")]
        public async Task<IActionResult> GetByCheckNum(string id) =>
            Ok((await genericrepository.GetAllAsync().ConfigureAwait(false)).FirstOrDefault(x => x.CheckNum == id));

        [HttpGet("GetByPayment/{id}")]
        public async Task<IActionResult> GetByPayment(long id) =>
            Ok(await royaltyrepository.GetCheckByIdAsync(id).ConfigureAwait(false));

        [HttpGet("GetByStorageRental/{id}")]
        public async Task<IActionResult> GetByStorageRental(long id) =>
          Ok(await repository.GetCheckByStorageRentalAsync(id).ConfigureAwait(false));

        [HttpGet("GetByContractRental/{id}")]
        public async Task<IActionResult> GetByContractRental(long id) =>
            Ok(await repository.GetCheckByContractRentalAsync(id).ConfigureAwait(false));

        [HttpGet("GetByOtherPayment/{id}")]
        public async Task<IActionResult> GetByOtherPayment(long id) =>
            Ok(await repository.GetCheckByOtherPaymentAsync(id).ConfigureAwait(false));

        [HttpGet("GetByUploadPayment/{id}")]
        public async Task<IActionResult> GetByUploadPayment(long id) =>
            Ok(await repository.GetCheckByUploadPaymentAsync(id).ConfigureAwait(false));

        [HttpPost]
        public IActionResult Post([FromBody]  Check model) =>
            Ok(checkBusinessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));
    }
}