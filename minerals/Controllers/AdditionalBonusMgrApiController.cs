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


    public class AdditionalBonusMgrApiController : ControllerBase
    {
        private readonly IAdditionalBonusRepository repository;

        public AdditionalBonusMgrApiController(IAdditionalBonusRepository repo) => repository = repo;

        [HttpGet("getbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetByContractAsync(id).ConfigureAwait(false));
    }
}