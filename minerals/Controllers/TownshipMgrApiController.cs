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
    public class TownshipMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<Township> genericRepository;
        private readonly ITownshipRepository townshipRepository;
        public TownshipMgrApiController(IGenericRepository<Township> genericRepo, ITownshipRepository townshipRepo)
        {
            genericRepository = genericRepo;
            townshipRepository = townshipRepo;
        }

        /// <summary> Returns all Townships  as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok(await genericRepository.GetAllAsync().ConfigureAwait(false));

        /// <summary> Returns all Townships given a contract id as JSON</summary>
        [HttpGet("townshipsbycontract/{id}")]
        public IActionResult  Get(long id) => Ok(townshipRepository.GetAllTownshipsByContract(id));
    }
}