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
    public class PaymentRequirementMgrApiController : ControllerBase
    {
        private readonly IPaymentRequirementRepository repository;
        public PaymentRequirementMgrApiController(IPaymentRequirementRepository repo) => repository = repo;

        /// <summary> Returns a payment requirement given a contract id as JSON</summary>
        [HttpGet("paymentRequirementbycontract/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetPaymentRequirementByContractAsync(id).ConfigureAwait(false));

        [HttpGet("paymentRequirementsbycontract/{id}")]
        public async Task<IActionResult> GetList(string id) =>
            Ok(await repository.GetPaymentRequirementsByContractAsync(id.Split(',').Select(s => long.Parse(s)).ToArray()).ConfigureAwait(false));

    }
}