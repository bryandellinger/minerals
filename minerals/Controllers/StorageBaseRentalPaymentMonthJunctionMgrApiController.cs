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

    public class StorageBaseRentalPaymentMonthJunctionMgrApiController : ControllerBase
    {
        private IStorageBaseRentalPaymentMonthJunctionRepository repository;
        public StorageBaseRentalPaymentMonthJunctionMgrApiController(IStorageBaseRentalPaymentMonthJunctionRepository repo) => repository = repo;

        [HttpGet("StorageBaseRentalPaymentMonthsByContract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetStorageBaseRentalPaymentMonthsByContractAsync(id).ConfigureAwait(false));

        [HttpGet("StorageBaseRentalPaymentMonthsByStorageRental/{id}")]
        public async Task<IActionResult> GetByStorageRental(long id) => Ok(await repository.GetStorageBaseRentalPaymentMonthsByStorageRentalAsync(id).ConfigureAwait(false));

    }
}