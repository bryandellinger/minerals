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
    public class StorageRentalMgrApiController : ControllerBase
    {
        private readonly IStorageRentalRepository repository;
        private readonly IGenericRepository<StorageRental> genericRepository;
        private readonly IStorageRentalBusinessLogic businessLogic;
        private readonly ICurrentUserRepository currentUserRepository;

        public StorageRentalMgrApiController(
            IStorageRentalRepository repo,
            IGenericRepository<StorageRental> genericRepo,
            IStorageRentalBusinessLogic storageRentalBusinessLogic,
            ICurrentUserRepository currentUserRepo
            )
        {
            repository = repo;
            genericRepository = genericRepo;
            businessLogic = storageRentalBusinessLogic;
            currentUserRepository = currentUserRepo;
        }

        [HttpGet()]
        public async Task<IActionResult> Get() => Ok((await repository.GetAllAsync().ConfigureAwait(false)));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericRepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("getByCheck/{id}")]
        public async Task<IActionResult> GetByCheck(long id) => Ok((await repository.GetByCheckAsync(id).ConfigureAwait(false)));

       [HttpPost]
        public IActionResult Post([FromBody]  StorageRental model) =>
            Ok(businessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));

    }
}