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
    public class LesseeHistoryMgrApiController : ControllerBase
    {

        private readonly ILesseeHistoryRepository repository;
        public LesseeHistoryMgrApiController(ILesseeHistoryRepository repo) => repository = repo;

        [HttpGet("lesseehistorybylessee/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetLesseeHistoryByLesseeAsync(id).ConfigureAwait(false));
    }
}