using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class RowContractMgrApiController : ControllerBase
    {
        private readonly IRowContractRepository repository;

        public RowContractMgrApiController(IRowContractRepository repo) => repository = repo;

        /// <summary> Returns all row contracts given a contract id as JSON</summary>
        [HttpGet("rowcontractsbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetAllRowContractsByContractAsync(id).ConfigureAwait(false));
    }
}