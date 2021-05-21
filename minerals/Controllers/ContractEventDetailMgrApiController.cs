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
    public class ContractEventDetailMgrApiController : ControllerBase
    {
        private readonly IContractEventDetailRepository repository;
        private long typoId;

        public ContractEventDetailMgrApiController(IContractEventDetailRepository repo,
            IGenericRepository<ContractEventDetailReasonForChange> reasonRepo)
        {
            repository = repo;
            typoId = reasonRepo.GetAll().First(x => x.Reason == Constants.ContractEventDetailReasonForChangeCorrection).Id;
        }

        /// <summary> Returns all event details given a contract id as JSON</summary>
        [HttpGet("eventdetailsbycontract/{id}")]
        public async Task<IActionResult> Get(long id) =>
            Ok((await repository.GetContractEventDetailsByContractAsync(id).ConfigureAwait(false)).Where(x => x.ContractEventDetailReasonForChangeId != typoId));

        [HttpGet("currenteventsbycontract/{id}")]
        public async Task<IActionResult> GetCurrentEvents(long id) =>
            Ok((await repository.GetContractEventDetailsByContractAsync(id).ConfigureAwait(false)).Where(x => x.ActiveInd == true));

    }
}