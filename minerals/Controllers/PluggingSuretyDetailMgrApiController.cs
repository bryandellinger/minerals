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
    public class PluggingSuretyDetailMgrApiController : ControllerBase
    {
        private readonly IPluggingSuretyDetailRepository repository;
        public PluggingSuretyDetailMgrApiController(IPluggingSuretyDetailRepository repo) => repository = repo;

        /// <summary> Returns a payment requirement given a contract id as JSON</summary>
        [HttpGet("pluggingsuretydetailsbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetPluggingSuretyDetailsByContractAsync(id).ConfigureAwait(false));
    }
}