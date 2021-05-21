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
    public class FileMgrApiController : ControllerBase
    {
        private IFileRepository repository;
        public FileMgrApiController(IFileRepository repo) => repository = repo;

        [HttpGet("filesbyunit/{id}")]
        public async Task<IActionResult> GetByUnit(long id) => Ok(await repository.GetFilesByUnitAsync(id).ConfigureAwait(false));

        [HttpGet("filesbycheck/{id}")]
        public async Task<IActionResult> GetByCheck(long id) => Ok(await repository.GetFilesByCheckAsync(id).ConfigureAwait(false));

        [HttpGet("filesbytemplate/{id}")]
        public async Task<IActionResult> GetByUploadTemplate(long id) => Ok(await repository.GetFilesByUploadTemplateAsync(id).ConfigureAwait(false));

        [HttpGet("filesbyuploadpayment/{id}")]
        public async Task<IActionResult> GetByUploadPayment(long id) => Ok(await repository.GetFilesByUploadPaymentAsync(id).ConfigureAwait(false));

    }
}