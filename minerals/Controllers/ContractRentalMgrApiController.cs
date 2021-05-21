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
    public class ContractRentalMgrApiController : ControllerBase
    {
        private readonly IContractRentalRepository repository;
        private readonly IGenericRepository<ContractRental> genericRepository;
        private readonly IContractRentalBusinessLogic businessLogic;
        private readonly ICurrentUserRepository currentUserRepository;

        public ContractRentalMgrApiController(IContractRentalRepository repo, 
            IGenericRepository<ContractRental> genericRepo,
            IContractRentalBusinessLogic contractRentalBusinessLogic,
           ICurrentUserRepository currentUserRepo)
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

        [HttpGet("getByCheck/{id}")]
        public async Task<IActionResult> GetByCheck(long id) => Ok((await repository.GetByCheckAsync(id).ConfigureAwait(false)));

        [HttpPost]
        public IActionResult Post([FromBody]  ContractRental model) =>
         Ok(businessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));
    }
}