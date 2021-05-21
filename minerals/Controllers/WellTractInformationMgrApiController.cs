using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class WellTractInformationMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<WellTractInformation> genericRepository;
        private readonly IWellTractInformationRepository repository;

        public WellTractInformationMgrApiController(IGenericRepository<WellTractInformation> genericrepo, IWellTractInformationRepository repo)
        {
            genericRepository = genericrepo;
            repository = repo;
        }


        [HttpGet("welltractinfobywell/{id}")]
        public async Task<IActionResult> Get(long id) => Ok((await genericRepository.GetAllAsync().ConfigureAwait(false)).Where(x => x.WellId == id));

        [HttpGet("welltractinfobycontract/{id}")]
        public async Task<IActionResult> GetbyContract(long id) => 
            Ok((await genericRepository.GetAllAsync().ConfigureAwait(false)).Where(x => x.ContractId == id && x.ActiveInd == true));

        [HttpGet("welltractinfobyroyalty/{id}")]
        public async Task<IActionResult> GetByRoyalty(long id) => Ok(await repository.GetByRoyaltyAsync(id).ConfigureAwait(false));

        [HttpGet("welltractinfosbylessee/{id}")]
        public async Task<IActionResult> GetByLessee(long id) => Ok(await repository.GetByLesseeAsync(id).ConfigureAwait(false));

        [HttpGet("welltractinfobyid/{id}")]
        public async Task<IActionResult> GetWellTractInfoById(long id) => Ok(await repository.GetWellTractInfoByIdAsync(id).ConfigureAwait(false));

    }
}