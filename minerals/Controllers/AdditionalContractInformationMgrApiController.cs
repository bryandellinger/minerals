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
    public class AdditionalContractInformationMgrApiController : ControllerBase
    {
        private IGenericRepository<AdditionalContractInformation> repository;

        public AdditionalContractInformationMgrApiController(IGenericRepository<AdditionalContractInformation> repo) =>
            repository = repo;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok((await repository.GetAllAsync().ConfigureAwait(false)).
            OrderBy(x => x.AdditionalContractInformationName));
    }
}