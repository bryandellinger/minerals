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
    public class LesseesContactMgrApiController : ControllerBase
    {
        private readonly ILesseeContactRepository  repository;
        private readonly IGenericRepository<LesseeContact> genericRepository;
        private readonly ILesseeContactBusinessLogic lesseeContactHelper;
        private readonly ICurrentUserRepository currentUserRepository;
        public LesseesContactMgrApiController(
            ILesseeContactRepository repo, IGenericRepository<LesseeContact> genericRepo, ILesseeContactBusinessLogic helper, ICurrentUserRepository currentUserRepo)
        {
            repository = repo;
            genericRepository = genericRepo;
            lesseeContactHelper = helper;
            currentUserRepository = currentUserRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(long id) => Ok(await genericRepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("lesseesContactsbylessee/{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await repository.GetLesseeContactsByLesseeAsync(id).ConfigureAwait(false));

        [HttpPost]
        public IActionResult Post([FromBody]  LesseeContact model) => 
            Ok(new LesseeContact { Id = lesseeContactHelper.AddLesseeContact(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id) });

        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            genericRepository.Delete(id);
            return Ok(new LesseeContact() { Id = 0 });
        }

    }
}