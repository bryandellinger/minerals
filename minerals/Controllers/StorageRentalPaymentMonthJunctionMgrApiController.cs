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

    public class StorageRentalPaymentMonthJunctionMgrApiController : ControllerBase
    {
        private IStorageRentalPaymentMonthJunctionRepository repository;
        public StorageRentalPaymentMonthJunctionMgrApiController(IStorageRentalPaymentMonthJunctionRepository repo) => repository = repo;

        [HttpGet("StorageRentalPaymentMonthsByContract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetStorageRentalPaymentMonthsByContractAsync(id).ConfigureAwait(false));

        [HttpGet("StorageRentalPaymentMonthsByStorageRental/{id}")]
        public async Task<IActionResult> GetByStorageRental(long id) => Ok(await repository.GetStorageRentalPaymentMonthsByStorageRentalAsync(id).ConfigureAwait(false));

    }
}