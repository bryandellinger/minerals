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
    public class UploadTemplateMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<UploadTemplate> genericrepository;
        private readonly ICurrentUserRepository currentUserRepository;
        private readonly IUploadTemplateBusinessLogic businessLogic;
        private readonly IUploadTemplateRepository repository;
        private readonly IGenericRepository<UploadTemplateMappedHeader> mappedHeaderRepository;
        private readonly IGenericRepository<UploadTemplateUnmappedHeader> unmappedHeaderRepository;

        public UploadTemplateMgrApiController(
         IGenericRepository<UploadTemplate> genericrepo,
         ICurrentUserRepository currentUserRepo,
         IUploadTemplateBusinessLogic uploadTemplateBusinessLogic,
         IUploadTemplateRepository repo,
         IGenericRepository<UploadTemplateMappedHeader> mappedHeaderRepo,
         IGenericRepository<UploadTemplateUnmappedHeader> unmappedHeaderRepo
         )
        {
            genericrepository = genericrepo;
            currentUserRepository = currentUserRepo;
            businessLogic = uploadTemplateBusinessLogic;
            repository = repo;
            mappedHeaderRepository = mappedHeaderRepo;
            unmappedHeaderRepository = unmappedHeaderRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await repository.GetAsync().ConfigureAwait(false));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericrepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("GetHeaders/{id}")]
        public async Task<IActionResult> GetHeaders(long id) => 
            Ok(await repository.GetHeadersAsync(id).ConfigureAwait(false));

        [HttpGet("GetMappedHeaders/{id}")]
        public async Task<IActionResult> GetMappedHeaders(long id) =>
            Ok((await mappedHeaderRepository.GetAllAsync().ConfigureAwait(false)).Where(x => x.UploadTemplateId == id));

        [HttpGet("GetUnmappedHeaders/{id}")]
        public async Task<IActionResult> GetUnmappedHeaders(long id) =>
           Ok((await unmappedHeaderRepository
               .GetAllAsync()
               .ConfigureAwait(false))
               .Where(x => x.UploadTemplateId == id)
               .OrderBy(x => x.Index));

        [HttpPost]
        public IActionResult Post([FromBody]  UploadTemplate model) =>
      Ok(businessLogic.Save(model, currentUserRepository.Get(HttpContext, User.Identity.Name).Id));
    }
}