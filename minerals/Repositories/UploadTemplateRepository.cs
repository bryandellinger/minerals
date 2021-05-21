using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class UploadTemplateRepository : IUploadTemplateRepository
    {
        private readonly DataContext context;

        public UploadTemplateRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetAsync() =>
          await  context.UploadTemplates
            .OrderBy(x => x.TemplateName)
            .Select(x => new
            {
                x.Id,
                x.TemplateName,
                x.TemplateNotes,
                x.Lessee.LesseeName,
                x.LesseeId
            })
            .ToListAsync()
            .ConfigureAwait(false);

        public async Task<object> GetHeadersAsync(long id)
        {
           var file = await context.Files.FindAsync(id);
            AppSetting appSetting = await context.AppSettings.Where(x => x.Key == Constants.AppSettingFileServer)
                                                    .FirstAsync().ConfigureAwait(false);

            string fileServer = appSetting.Value;
            var uploadPath = Path.Combine(fileServer, Constants.UploadFolder);
            var path = Path.Combine(uploadPath, $"{file.FileGuid.ToString()}{file.FileExtension}");

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                string[] headerRow = csv.HeaderRecord;
                return headerRow;
            }

        }

        public void Update(UploadTemplate model, long userId)
        {
            var uploadTemplate = context.UploadTemplates.Find(model.Id);
            uploadTemplate.TemplateName = model.TemplateName;
            uploadTemplate.LesseeId = model.LesseeId;
            uploadTemplate.TemplateNotes = model.TemplateNotes;
            uploadTemplate.LastUpdateDate = DateTime.Now;
            uploadTemplate.UpdatedBy = userId;
            context.SaveChanges();
        }

        public void UpdateFiles(long id, IEnumerable<Models.File> files)
        {
            List<Models.File> uploadTemplateFiles = context.UploadTemplates.Include(x => x.Files).FirstOrDefault(x => x.Id == id).Files.ToList();
            foreach (var file in uploadTemplateFiles)
            {
                context.Files.Remove(file);
                context.SaveChanges();
            }

            var uploadTemplate = context.UploadTemplates.Find(id);

            List<Models.File> newFiles = new List<Models.File>();
            foreach (var item in files)
            {
                newFiles.Add(new Models.File
                {
                    FileExtension = item.FileExtension,
                    FileName = item.FileName,
                    FileGuid = item.FileGuid,
                    FileSize = item.FileSize,
                    FileIcon = item.FileIcon
                });
            }
            uploadTemplate.Files = newFiles;
            context.SaveChanges();
        }
    }
}
