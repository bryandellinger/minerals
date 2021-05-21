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

    public class StorageWellPaymentMonthJunctionMgrApiController : ControllerBase
    {
        private IStorageWellPaymentMonthJunctionRepository repository;

        public StorageWellPaymentMonthJunctionMgrApiController(IStorageWellPaymentMonthJunctionRepository repo)
            => repository = repo;

        [HttpGet("StorageWellPaymentMonthsByContract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetStorageWellPaymentMonthsByContractAsync(id).ConfigureAwait(false));

        [HttpGet("StorageWellPaymentMonthsByStorageRental/{id}")]
        public async Task<IActionResult> GeByStorageRental(long id) => Ok(await repository.GetStorageWellPaymentMonthsByStorageRentalAsync(id).ConfigureAwait(false));
    }
}