using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Interfaces;
using Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMgrApiController : ControllerBase
    {
        private readonly IUserRepository repository;

        public UserMgrApiController(IUserRepository repo) => repository = repo;

        /// <summary>given id update user</summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(long id, [FromBody] User updatedUser)
        {
            if (updatedUser == null || id != updatedUser.Id)
            {
                return BadRequest("please supply a user, and an id ");
            }

            User user = await repository.FindAsync(id);

            if (user == null)
            {
                return BadRequest($"unable to find user with an id of {id.ToString(new CultureInfo("en-US", false))}");
            }

            user.NameFirst = updatedUser.NameFirst;
            user.NameLast = updatedUser.NameLast;
            user.JobTitle = updatedUser.JobTitle;
            user.LastUpdateDate = DateTime.Now;
            user.WorkAddr = updatedUser.WorkAddr;
            user.WorkPhone = updatedUser.WorkPhone;
            user.Company = updatedUser.Company;
            user.EmailAddress = updatedUser.EmailAddress;


            await repository.UpdateAsync(user, HttpContext).ConfigureAwait(false);
            return Ok(user);
        }
        /// <summary>return all users as JSON</summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await repository.GetAllAsync().ConfigureAwait(false);
            return Ok(result);
        }



    }
}