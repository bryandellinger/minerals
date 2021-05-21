using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class CheckRepository : ICheckRepository
    {
        private readonly DataContext context;

        public CheckRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetCheckByContractRentalAsync(long id) =>
       await context.ContractRentals
              .Where(x => x.Id == id)
              .Select(o => new
              {
                  o.Check.Id,
                  o.Check.CheckNum,
                  o.Check.CheckDate,
                  o.Check.ReceivedDate,
                  o.Check.LesseeName,
                  o.Check.LesseeId
    })
              .FirstOrDefaultAsync()
              .ConfigureAwait(false);

        public async Task<object> GetCheckByOtherPaymentAsync(long id) =>
    await context.OtherRentals
           .Where(x => x.Id == id)
           .Select(o => new
           {
               o.Check.Id,
               o.Check.CheckNum,
               o.Check.CheckDate,
               o.Check.ReceivedDate,
               o.Check.LesseeName,
               o.Check.LesseeId
           })
           .FirstOrDefaultAsync()
           .ConfigureAwait(false);

        public async Task<object> GetCheckByUploadPaymentAsync(long id) =>
   await context.UploadPayments
          .Where(x => x.Id == id)
          .Select(o => new
          {
              o.Check.Id,
              o.Check.CheckNum,
              o.Check.CheckDate,
              o.Check.ReceivedDate,
              o.Check.LesseeName,
              o.Check.LesseeId,
              o.Check.TotalAmount,
          })
          .FirstOrDefaultAsync()
          .ConfigureAwait(false);

        public async Task<object> GetCheckByStorageRentalAsync(long id) =>
         await context.StorageRentals
              .Where(x => x.Id == id)
              .Select(o => new
              {
                  o.Check.Id,
                  o.Check.CheckNum,
                  o.Check.CheckDate,
                  o.Check.ReceivedDate,
                  o.Check.LesseeName,
                  o.Check.LesseeId
              })
              .FirstOrDefaultAsync()
              .ConfigureAwait(false);

        public async Task<object> GetSelect2Data(string id) =>
          await context.Checks
            .Where(x => x.CheckNum.StartsWith(id))
            .OrderBy(x => x.CheckNum)
            .Select(c => new
            {
                c.Id,
                Text = c.CheckNum
            })
            .ToListAsync()
            .ConfigureAwait(false);
        

        public void Update(Check model, long userId)
        {
            var check = context.Checks.Find(model.Id);
            check.LesseeId = model.LesseeId;
            check.Notes = model.Notes;
            check.LesseeName = model.LesseeName;
            check.TotalAmount = model.TotalAmount;
            check.CheckNum = model.CheckNum;
            check.ReceivedDate = model.ReceivedDate;
            check.CheckDate = model.CheckDate;
            check.LastUpdateDate = DateTime.Now;
            check.UpdatedBy = userId;
            context.SaveChanges();

        }

        public void UpdateFiles(long id, IEnumerable<File> files)
        {
            List<File> checkFiles = context.Checks.Include(x => x.Files).FirstOrDefault(x => x.Id == id).Files.ToList();
            foreach (var file in checkFiles)
            {
                context.Files.Remove(file);
                context.SaveChanges();
            }

            var check2 = context.Checks.Find(id);

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
            check2.Files = newFiles;
            context.SaveChanges();

        }
    }
}
