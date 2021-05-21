
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Models;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class PaymentTypeMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<PaymentType> repository;

        public PaymentTypeMgrApiController(IGenericRepository<PaymentType> repo) => repository = repo;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok((await repository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.PaymentTypeName));

    }
}