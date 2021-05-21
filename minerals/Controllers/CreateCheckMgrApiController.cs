using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class CreateCheckMgrApiController : ControllerBase
    {
        private readonly IUploadPaymentBusinessLogic businessLogic;
        private readonly ICurrentUserRepository currentUserRepository;

        public CreateCheckMgrApiController(
            IUploadPaymentBusinessLogic contractRentalBusinessLogic,
            ICurrentUserRepository currentUserRepo)
        {
            businessLogic = contractRentalBusinessLogic;
            currentUserRepository = currentUserRepo;
        }

        [HttpPost]
        public IActionResult Post([FromBody]  CreateCheckViewModel model) =>
         Ok(businessLogic.CreateCheck(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));
    }
}