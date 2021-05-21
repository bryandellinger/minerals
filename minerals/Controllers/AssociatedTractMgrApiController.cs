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
    public class AssociatedTractMgrApiController : ControllerBase
    {
        private readonly IAssociatedTractRepository repository;

        public AssociatedTractMgrApiController(IAssociatedTractRepository repo) => repository = repo;

        [HttpGet("associatedtractsbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetAllAssociatedTractsByContractAsync(id).ConfigureAwait(false));
    }
}