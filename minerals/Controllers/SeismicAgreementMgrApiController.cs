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
    public class SeismicAgreementMgrApiController : ControllerBase
    {
        private readonly ISeismicAgreementRepository repository;
        public SeismicAgreementMgrApiController(ISeismicAgreementRepository repo) => repository = repo;

        [HttpGet("seismicagreementbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetSeismicAgreementByContractAsync(id).ConfigureAwait(false));
    }
}