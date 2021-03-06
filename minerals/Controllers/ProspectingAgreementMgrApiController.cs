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
    public class ProspectingAgreementMgrApiController : ControllerBase
    {
        private readonly IProspectingAgreementRepository repository;
        public ProspectingAgreementMgrApiController(IProspectingAgreementRepository repo) => repository = repo;

        [HttpGet("prospectingagreementbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetProspectingAgreementByContractAsync(id).ConfigureAwait(false));
    }
}