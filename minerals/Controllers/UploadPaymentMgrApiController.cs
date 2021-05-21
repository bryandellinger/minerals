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
    public class UploadPaymentMgrApiController : ControllerBase
    {
        private readonly IUploadPaymentRepository repository;
        private readonly IGenericRepository<UploadPayment> genericRepository;
        private readonly IUploadPaymentBusinessLogic businessLogic;
        private readonly ICurrentUserRepository currentUserRepository;

        public UploadPaymentMgrApiController(
            IUploadPaymentRepository repo,
            IGenericRepository<UploadPayment> genericRepo,
            IUploadPaymentBusinessLogic contractRentalBusinessLogic,
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

        [HttpGet("CreateCSVPayments/{fileId}/{uploadTemplateId}")]
        public ActionResult Get(long fileId, long uploadTemplateId) => Ok(businessLogic.CreateCSVPayments(fileId, uploadTemplateId));

        [HttpGet("GetCSVPayments/{id}")]
        public async Task<IActionResult> GetCSVPaymentsAsyync(long id) => Ok(await repository.GetCSVPaymentsAsyync(id).ConfigureAwait(false));

        [HttpPost]
        public IActionResult Post([FromBody]  UploadPayment model) =>
            Ok(businessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));
    }
}