using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class RoyaltyAdjustmentCardApiController : ControllerBase
    {
        private readonly IRoyaltyAdjustmentCardViewModelRepository repository;

        public RoyaltyAdjustmentCardApiController(IRoyaltyAdjustmentCardViewModelRepository repo) => repository = repo;

        /// <summary> Return Royalty Adjustment Information as JSON given a search parameter as a string </summary>
        [HttpGet("{search}")]
        public Task<IEnumerable<RoyaltyAdjustmentCardViewModel>> Get(string search)
        {
            return repository.Get(search == "null" ? null : search);
        }


        /// <summary> Return Royalty Adjustment Information as JSON given a search parameter as an object JSON</summary>
        [AuthorizeUser(Roles = "read")]
        [HttpPost]
        public Task<IEnumerable<RoyaltyAdjustmentCardViewModel>> Post([FromBody]  RoyaltyAdjustmentCardViewModel model)
        {
            return repository.Search(model);
        }

    }
}