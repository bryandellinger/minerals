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
    public class WellStatusApiController : ControllerBase
    {
        private readonly IGenericRepository<WellStatus> repository;

        public WellStatusApiController(IGenericRepository<WellStatus> repo) => repository = repo;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok((await repository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.WellStatusName));
    }
}