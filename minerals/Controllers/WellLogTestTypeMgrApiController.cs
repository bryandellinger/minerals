using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class WellLogTestTypeMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<WellLogTestType> WellLogTestTypeRepository;
        private readonly IGenericRepository<DigitalWellLogTestTypeWellJunction> DigitalWellLogTestTypeWellJunctionRepository;
        private readonly IGenericRepository<DigitalImageWellLogTestTypeWellJunction> DigitalImageWellLogTestTypeWellJunctionRepository;
        private readonly IGenericRepository<HardCopyWellLogTestTypeWellJunction> HardCopyWellLogTestTypeWellJunctionRepository;

        public WellLogTestTypeMgrApiController(IGenericRepository<WellLogTestType> WellLogTestTypeRepo,
                                               IGenericRepository<DigitalWellLogTestTypeWellJunction> DigitalWellLogTestTypeWellJunctionRepo,
                                               IGenericRepository<DigitalImageWellLogTestTypeWellJunction> DigitalImageWellLogTestTypeWellJunctionRepo,
                                               IGenericRepository<HardCopyWellLogTestTypeWellJunction> HardCopyWellLogTestTypeWellJunctionRepo
                                               )
        {
            WellLogTestTypeRepository = WellLogTestTypeRepo;
            DigitalWellLogTestTypeWellJunctionRepository = DigitalWellLogTestTypeWellJunctionRepo;
            DigitalImageWellLogTestTypeWellJunctionRepository = DigitalImageWellLogTestTypeWellJunctionRepo;
            HardCopyWellLogTestTypeWellJunctionRepository = HardCopyWellLogTestTypeWellJunctionRepo;
        }

        [HttpGet()]
        public async Task<IActionResult> Get() =>
            Ok((await WellLogTestTypeRepository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.WellLogTestName));

        [HttpGet("digitallogsbywell/{id}")]
        public async Task<IActionResult> GetDigital(long id) => 
            Ok((await DigitalWellLogTestTypeWellJunctionRepository.GetAllAsync().ConfigureAwait(false))
                .Where(x => x.WellId == id)
                .Select(x => x.WellLogTestTypeId));

        [HttpGet("digitalimagelogsbywell/{id}")]
        public async Task<IActionResult> GetDigitalImage(long id) =>
           Ok((await DigitalImageWellLogTestTypeWellJunctionRepository.GetAllAsync().ConfigureAwait(false))
               .Where(x => x.WellId == id)
               .Select(x => x.WellLogTestTypeId));

        [HttpGet("hardcopylogsbywell/{id}")]
       public async Task<IActionResult> GetHardcopy(long id) =>
        Ok((await HardCopyWellLogTestTypeWellJunctionRepository.GetAllAsync().ConfigureAwait(false))
          .Where(x => x.WellId == id)
          .Select(x => x.WellLogTestTypeId));

    }
}