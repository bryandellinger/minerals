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
    public class SurfaceUseAgreementMgrApiController : ControllerBase
    {
        private readonly ISurfaceUseAgreementRepository repository;
        public SurfaceUseAgreementMgrApiController(ISurfaceUseAgreementRepository repo) => repository = repo;

        [HttpGet("surfaceuseagreementbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetSurfaceUseAgreementByContractAsync(id).ConfigureAwait(false));
    }
}