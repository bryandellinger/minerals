using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class UploadPaymentRepository : IUploadPaymentRepository
    {
        private readonly DataContext context;
        public UploadPaymentRepository(DataContext ctx) => context = ctx;

        public void AddCSVFiles(long id, IEnumerable<CSVPayment> cSVPayments)
        {
            foreach (var item in cSVPayments)
            {
                item.Id = 0;
                item.UploadPaymentId = id;
            }
            context.CSVPayments.AddRange(cSVPayments);
            context.SaveChanges();
        }

        public void DeleteCSVFiles(long Id)
        {
            var csvFilesToDelete = context.CSVPayments.Where(x => x.UploadPaymentId == Id);
            context.CSVPayments.RemoveRange(csvFilesToDelete);
            context.SaveChanges();
        }

        public async Task<object> GetAllAsync() =>
         await (from uploadPayments in context.UploadPayments.Include(x => x.Files)
                join uploadTemplates in context.UploadTemplates on uploadPayments.UploadTemplateId equals uploadTemplates.Id
                join lessees in context.Lessees on uploadTemplates.LesseeId equals lessees.Id
                join checks in context.Checks on uploadPayments.CheckId equals checks.Id into cu
                from checks in cu.DefaultIfEmpty()
                select new
                {
                    uploadPayments.Id,
                    uploadPayments.Files.FirstOrDefault().FileName,
                    uploadPayments.UploadPaymentEntryDate,
                    uploadTemplates.TemplateName,
                    lessees.LesseeName,
                    CheckNum = checks == null ? string.Empty : checks.CheckNum,
                    TotalAmount = checks == null ? (decimal?)null : checks.TotalAmount,
                })
         .ToListAsync()
         .ConfigureAwait(false);

        public async Task<object> GetCSVPaymentsAsyync(long id)
            => await context.CSVPayments.Where(x => x.UploadPaymentId == id).ToListAsync().ConfigureAwait(false);
      

        public void Update(UploadPayment model, long userId)
        {
            var uploadPayment = context.UploadPayments.Find(model.Id);
            uploadPayment.LastUpdateDate = DateTime.Now;
            uploadPayment.UpdatedBy = userId;
            uploadPayment.UploadTemplateId = model.UploadTemplateId;
            uploadPayment.UploadPaymentNotes = model.UploadPaymentNotes;
            uploadPayment.UploadPaymentEntryDate = model.UploadPaymentEntryDate;
            uploadPayment.CheckId = model.CheckId;
            context.SaveChanges();
        }

        public void UpdateFiles(long id, IEnumerable<File> files)
        {
            List<Models.File> uploadPaymentFiles = context.UploadPayments.Include(x => x.Files).FirstOrDefault(x => x.Id == id).Files.ToList();
            foreach (var file in uploadPaymentFiles)
            {
                context.Files.Remove(file);
                context.SaveChanges();
            }

            var uploadPayment = context.UploadPayments.Find(id);

            List<File> newFiles = new List<File>();
            foreach (var item in files)
            {
                newFiles.Add(new File
                {
                    FileExtension = item.FileExtension,
                    FileName = item.FileName,
                    FileGuid = item.FileGuid,
                    FileSize = item.FileSize,
                    FileIcon = item.FileIcon
                });
            }
            uploadPayment.Files = newFiles;
            context.SaveChanges();
        }
    }
}
