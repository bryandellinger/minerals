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
    public class DistrictTractJunctionMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<DistrictTractJunction> repository;

        public DistrictTractJunctionMgrApiController(IGenericRepository<DistrictTractJunction> repo ) => repository = repo;

        [HttpGet()]
        public async Task<IActionResult> Get() => Ok(await repository.GetAllAsync().ConfigureAwait(false));

        [HttpGet("junctionsbytract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok((await repository.GetAllAsync().ConfigureAwait(false)).Where(x => x.TractId == id));


    }
}