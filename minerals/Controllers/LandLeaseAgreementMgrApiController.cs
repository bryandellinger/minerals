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
    public class LandLeaseAgreementMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<LandLeaseAgreement> genericrepository;
        private readonly ILandLeaseAgreementRepository landleaseagreementrepository;

        public LandLeaseAgreementMgrApiController(
            IGenericRepository<LandLeaseAgreement> genericrepo,
            ILandLeaseAgreementRepository  landleaseagreementrepo 
            )
            {
            genericrepository = genericrepo;
            landleaseagreementrepository = landleaseagreementrepo;
            }

        /// <summary> Returns all Contract Types as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok(await genericrepository.GetAllAsync().ConfigureAwait(false));

        [HttpGet("landleaseagreementbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await landleaseagreementrepository.GetLandLeaseAgreementByContractAsync(id).ConfigureAwait(false));
    }
}