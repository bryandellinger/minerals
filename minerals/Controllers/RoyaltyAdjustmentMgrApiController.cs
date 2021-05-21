using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class RoyaltyAdjustmentMgrApiController : ControllerBase
    {
    
        private readonly ICurrentUserRepository currentUserRepository;
        private readonly IRoyaltyAdjustmentRepository repository;

        public RoyaltyAdjustmentMgrApiController(
           ICurrentUserRepository currentUserRepo,
           IRoyaltyAdjustmentRepository repo)
        {
            currentUserRepository = currentUserRepo;
            repository = repo;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]  RoyaltyAdjustment model, long id)
        {           
            RoyaltyAdjustment royaltyAdjustment = repository.Update(model, id, currentUserRepository.Get(HttpContext, User.Identity.Name).Id);
            return Ok(new Royalty { Id = royaltyAdjustment.RoyaltyId.Value});
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            repository.Delete(id);
            return Ok(new Royalty());

        }


    }
}