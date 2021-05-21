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
    public class AltIdCategoryMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<AltIdCategory> genericrepository;
        private readonly IAssociatedContractRepository associatedContractRepository;
        public AltIdCategoryMgrApiController(IGenericRepository<AltIdCategory> genericrepo, IAssociatedContractRepository associatedContractRepo)
        {
            genericrepository = genericrepo;
            associatedContractRepository = associatedContractRepo;
        }

        /// <summary> Returns all Alt Id Categories  as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok(await genericrepository.GetAllAsync().ConfigureAwait(false));

        [HttpGet("GetAssociatedContractsDropDownList")]
        public IActionResult GetAssociatedContractsDropDownList() => Ok(associatedContractRepository.GetAssociatedContractsDropDownList());
    }
}