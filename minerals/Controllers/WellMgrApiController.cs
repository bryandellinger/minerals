using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class WellMgrApiController : ControllerBase
    {
        private readonly IWellRepository repository;
        private readonly IGenericRepository<Well> genericRepository;
        private readonly IGenericRepository<WellTractInformation> wellTractInfoRepository;
        private readonly IWellBusinessLogic wellBusinessLogic;
        private readonly ICurrentUserRepository currentUserRepository;

        public WellMgrApiController(IWellRepository repo,
            IGenericRepository<Well> genericRepo,
            IGenericRepository<WellTractInformation> wellTractInfoRepo,
            IWellBusinessLogic businessLogic,
            ICurrentUserRepository currentUserRepo
            )
        {
            repository = repo;
            genericRepository = genericRepo;
            wellBusinessLogic = businessLogic;
            currentUserRepository = currentUserRepo;
            wellTractInfoRepository = wellTractInfoRepo;
        }

        /// <summary>given pad id return all pads as JSON</summary>
        [HttpGet("wellsbypad/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetAllWellsByPadAsync(id).ConfigureAwait(false));

        /// <summary>get all wells as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok((await genericRepository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.ApiNum));

        [HttpGet("wellsForDataTable")]
        public IActionResult GetWellsForDataTable() => Ok(repository.GetWellsForDataTable());

        [HttpGet("{id}")]
        public IActionResult GetWellById(long id) => Ok( repository.GetWellById(id));

        [HttpGet("wellsbycontract/{id}")]
        public async Task<IActionResult> GetbyContract(long id)
        {
            List<Well> retval = new List<Well>();
            var wells = (await genericRepository.GetAllAsync().ConfigureAwait(false));

            long[] wellIds = (await wellTractInfoRepository.GetAllAsync().ConfigureAwait(false))
                .Where(x => x.ContractId != null && x.ContractId.Value == id && x.ActiveInd == true)
                .Select(x => x.WellId).Distinct().ToArray();

            foreach (long wellid in wellIds)
            {
                var w = wells.First(x => x.Id == wellid);
                retval.Add(new Well(){ Id = w.Id, WellNum = w.WellNum, ApiNum = w.ApiNum, AutoUpdatedAllowedInd = w.AutoUpdatedAllowedInd });
            }
            return Ok(retval);
           
        }

        [HttpPost]
        public IActionResult Post([FromBody]  WellViewModel model) =>
            Ok(wellBusinessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));

        [HttpPut]
        public IActionResult Put([FromBody] WellViewModel model) => Ok(repository.UpdateAutoUpdatedAllowedInd(model));

        [HttpPut("updatebycontract")]
        public IActionResult PutbyContract([FromBody] WellViewModel model) => Ok(repository.UpdateAllAutoUpdatedAllowedInd(model));

    }
}