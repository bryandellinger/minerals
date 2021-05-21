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
    public class ContractRentalPaymentMonthJunctionMgrApiController : ControllerBase
    {
        private IContractRentalPaymentMonthJunctionRepository repository;
        public ContractRentalPaymentMonthJunctionMgrApiController(IContractRentalPaymentMonthJunctionRepository repo)
            => repository = repo;

        [HttpGet("ContractRentalPaymentMonthsByContract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetContractRentalPaymentMonthsByContractAsync(id).ConfigureAwait(false));

        [HttpGet("ContractRentalPaymentMonthsByContractRental/{id}")]
        public async Task<IActionResult> GetContractRentalPaymentMonthsByContractRentalAsync(long id) =>
            Ok(await repository.GetContractRentalPaymentMonthsByContractRentalAsync(id).ConfigureAwait(false));
    }
}