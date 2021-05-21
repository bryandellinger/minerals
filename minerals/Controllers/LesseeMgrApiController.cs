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
    public class LesseeMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<Lessee> genericrepository;
        private readonly ILesseeRepository lesseerepository;
        private readonly ICurrentUserRepository currentUserRepository;
        private readonly IContractEventDetailRepository contractEventDetailRepository;
        private readonly ILesseeBusinessLogic lesseeBusinessLogic;

        public LesseeMgrApiController(IGenericRepository<Lessee> genericrepo, 
            ILesseeRepository lesseerepo,
            ICurrentUserRepository currentUserRepo,
            IContractEventDetailRepository contractEventDetailRepo,
            ILesseeBusinessLogic businessLogic)
        {
            genericrepository = genericrepo;
            lesseerepository = lesseerepo;
            currentUserRepository = currentUserRepo;
            contractEventDetailRepository = contractEventDetailRepo;
            lesseeBusinessLogic = businessLogic;
        }

        /// <summary> Returns a Lessee object as JSON given an id</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericrepository.GetByIdAsync(id).ConfigureAwait(false));

        /// <summary> Returns all Lessees as JSON </summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => 
            Ok((await genericrepository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.LesseeName));

        [HttpGet("getByPayment/{id}")]
        public async Task<IActionResult> GetByPayment(long id) => Ok(await lesseerepository.GetByPaymentIdAsync(id).ConfigureAwait(false));

        [HttpGet("GetByStorageRental/{id}")]
        public async Task<IActionResult> GetByStorageRental(long id) => Ok(await lesseerepository.GetByStorageRental(id).ConfigureAwait(false));

        [HttpGet("GetByContractRental/{id}")]
        public async Task<IActionResult> GetByContractRental(long id) => Ok(await lesseerepository.GetByContractRental(id).ConfigureAwait(false));

        [HttpGet("GetByOtherPayment/{id}")]
        public async Task<IActionResult> GetByOtherPaymentl(long id) => Ok(await lesseerepository.GetByOtherPayment(id).ConfigureAwait(false));

        [HttpGet("GetByUploadPayment/{id}")]
        public async Task<IActionResult> GetByUploadPaymentl(long id) => Ok(await lesseerepository.GetByUploadPayment(id).ConfigureAwait(false));

        [HttpPost]
        public IActionResult Post([FromBody]  Lessee model)
        {
            Lessee insertedLessee = genericrepository.Insert(new Lessee
            {
              Id = 0,
              LesseeName = model.LesseeName,
              LogicalDeleteIn = false,
            });
            return Ok(insertedLessee);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]  EditLesseeViewModel model, long id)
        {
            Lessee updatedLessee = lesseerepository.Update(id, model, currentUserRepository.Get(HttpContext, User.Identity.Name));
            if (model.AddHistoryInd)
            {
                lesseeBusinessLogic.CreateHistoricalContractEventDetailRecords(id);
            }
            contractEventDetailRepository.UpdateCurrentContractEventDetailLeseeNamesByLesseeId(id, model.LesseeName);
            lesseeBusinessLogic.CreateHistoricalWellOwnershipRecords(id, model.AddHistoryInd);
            return Ok(new Lessee { Id = updatedLessee.Id, LesseeName = updatedLessee.LesseeName });
        }

    }
}