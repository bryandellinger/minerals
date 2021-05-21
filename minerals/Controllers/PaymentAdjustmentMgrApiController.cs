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
    public class PaymentAdjustmentMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<RoyaltyAdjustment> genericrepository;
        private readonly IRoyaltyAdjustmentRepository repository;

        public PaymentAdjustmentMgrApiController(
            IGenericRepository<RoyaltyAdjustment> genericrepo,
            IRoyaltyAdjustmentRepository repo
            )
        {
            genericrepository = genericrepo;
            repository = repo;
        }

        [HttpGet("adjustmentsbypayment/{id}")]
        public async Task<IActionResult> GetByPayment(long id) => Ok(await repository.GetByPaymentAsync(id).ConfigureAwait(false));

        [HttpGet("adjustmentsbycheck/{id}")]
        public async Task<IActionResult> GetByCheck(long id) => Ok(await repository.GetByCheckAsync(id).ConfigureAwait(false));


    }
}
  