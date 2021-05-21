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
    public class PaymentMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<Royalty> genericrepository;
        private readonly IRoyaltyRepository repository;
        private readonly ICurrentUserRepository currentUserRepository;
        private readonly IPaymentBusinessLogic paymentBusinessLogic;

        public PaymentMgrApiController(
            IGenericRepository<Royalty> genericrepo,
            IRoyaltyRepository repo,
            ICurrentUserRepository currentUserRepo,
            IPaymentBusinessLogic businessLogic
            )
        {
            genericrepository = genericrepo;
            repository = repo;
            currentUserRepository = currentUserRepo;
            paymentBusinessLogic = businessLogic;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericrepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("paymentsbycheck/{id}")]
        public async Task<IActionResult> GetByCheck(long id) => Ok(await repository.GetByCheck(id).ConfigureAwait(false));

        [HttpGet("CheckProdMonth/{lesseeId}/{postMonth}/{postYear}/{paymentTypeId}/{wellId}")]
        public async Task<IActionResult> CheckProdMonth(long lesseeId, int postMonth, int postYear, long paymentTypeId, long wellId )
            => Ok(await repository.CheckProdMonthAsync(lesseeId, postMonth, postYear, paymentTypeId, wellId).ConfigureAwait(false));

        [HttpPost]
        public IActionResult Post([FromBody]  RoyaltyViewModel model) =>
        Ok(paymentBusinessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));
    }
}