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


    public class ApiCodeMgrApiController : ControllerBase
    {
        private readonly IApiCodeRepository repository;
        public ApiCodeMgrApiController (IApiCodeRepository repo) => repository = repo;

        [HttpGet("statecodes")]
        public async Task<IActionResult> Get() => Ok(await repository.GetStateCodes().ConfigureAwait(false));

        [HttpGet("countycodes/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetCountyCodes(id).ConfigureAwait(false));



    }
}