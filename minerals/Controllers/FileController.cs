using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;

namespace Minerals.Controllers
{
    public class FileController : Controller
    {
        private string fileServer;
        private IGenericRepository<Models.File> repository;
        private IGenericRepository<EventLog> eventLogRepository;
        private FileTypeData fileTypeData;

        public FileController(IGenericRepository<AppSetting> apRepo,
            IGenericRepository<Models.File> fileRepo,
            FileTypeData ftd,
            IGenericRepository<EventLog> eventLogRepo)
        {
            fileServer = apRepo.GetAll().First(x => x.Key == Constants.AppSettingFileServer).Value;
            repository = fileRepo;
            fileTypeData = ftd;
            eventLogRepository = eventLogRepo;
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                Models.File insertedFile = new Models.File();
                if (file.Length > 0)              
                {
                    string folderRoot = Path.Combine(fileServer, Constants.UploadFolder);
                    Guid fileGuid = Guid.NewGuid();
                    string fileExtension = Path.GetExtension(file.FileName);
                    string fileTypeIcon = Constants.DefaultFileIcon;
                    FileTypeIcon fti = fileTypeData.fileTypeIcons.FirstOrDefault(x => x.FileExtension == fileExtension);
                    if (fti != null)
                    {
                        fileTypeIcon = fti.Icon;
                    }
                    string filePath = fileGuid + fileExtension;
                    filePath = Path.Combine(folderRoot, filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream).ConfigureAwait(false);
                    }
                    insertedFile = repository.Insert(
                        new Models.File {
                            Id = 0, FileGuid = fileGuid,
                            FileName = file.FileName,
                            FileExtension = fileExtension,
                            FileSize = file.Length,
                            FileIcon = fileTypeIcon
                        });
                }
                return Ok(insertedFile);
            }
            catch (Exception ex)
            {
                eventLogRepository.Insert(new EventLog
                {
                    EventMessage = ex?.Message + ex?.InnerException,
                    Type = "ApplicationError",
                    Source = ex?.StackTrace,
                    StatusCode = "Error",
                    CreateDate = DateTime.Now
                });
                throw;
               
            }
        }

        [HttpGet]
        public IActionResult DownloadFile(long id)
        {
            var uploadPath = Path.Combine(fileServer, Constants.UploadFolder);
            Models.File file = repository.GetById(id);

            var path = Path.Combine(uploadPath, $"{file.FileGuid.ToString()}{file.FileExtension}");
            var fs = new FileStream(path, FileMode.Open);
            return File(fs, "application/octet-stream", file.FileName);
        }
    }
}