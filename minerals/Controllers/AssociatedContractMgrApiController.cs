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
    public class AssociatedContractMgrApiController : ControllerBase
    {
        private readonly IAssociatedContractRepository repository;

        public AssociatedContractMgrApiController(IAssociatedContractRepository repo) => repository = repo;

        /// <summary> Returns all associated contracts given a contract id as JSON</summary>
        [HttpGet("associatedcontractsbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetAllAssociatedContractsByContractAsync(id).ConfigureAwait(false));
    }
}