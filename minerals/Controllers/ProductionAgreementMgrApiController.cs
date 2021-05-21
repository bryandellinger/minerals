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
    public class ProductionAgreementMgrApiController : ControllerBase
    {
        private readonly IProductionAgreementRepository repository;
        public ProductionAgreementMgrApiController(IProductionAgreementRepository repo) => repository = repo;

        [HttpGet("productionagreementbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetProductionAgreementByContractAsync(id).ConfigureAwait(false));
    }
}