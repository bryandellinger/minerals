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
    public class LesseeRepository : ILesseeRepository
    {
        private readonly DataContext context;
        public LesseeRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetByPaymentIdAsync(long id)
        {
            Royalty royalty = await context.Royalties.Include(x => x.Check).ThenInclude(x => x.Lessee).Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
            if (royalty != null && royalty.Check != null && royalty.Check.Lessee != null)
            {
                return royalty.Check.Lessee;
            }
            else
            {
                return new Lessee();
            }
        }

        public async Task<object> GetByStorageRental(long id) =>  
            await context.StorageRentals
              .Where(x => x.Id == id)
              .Select(o => new
              {
                  o.Check.LesseeId,
                  o.Check.Lessee.LesseeName,
              })
              .FirstOrDefaultAsync()
              .ConfigureAwait(false);

        public async Task<object> GetByContractRental(long id) =>
        await context.ContractRentals
          .Where(x => x.Id == id)
          .Select(o => new
          {
              o.Check.LesseeId,
              o.Check.Lessee.LesseeName,
          })
          .FirstOrDefaultAsync()
          .ConfigureAwait(false);

        public async Task<object> GetByOtherPayment(long id) =>
     await context.OtherRentals
       .Where(x => x.Id == id)
       .Select(o => new
       {
           o.Check.LesseeId,
           o.Check.Lessee.LesseeName,
       })
       .FirstOrDefaultAsync()
       .ConfigureAwait(false);

        public async Task<object> GetByUploadPayment(long id) =>
            await context.UploadPayments
            .Where(x => x.Id == id)
            .Select(o => new
            {
                o.Check.LesseeId,
                o.Check.Lessee.LesseeName,
            })
             .FirstOrDefaultAsync()
            .ConfigureAwait(false);



        public Lessee Update(long id, EditLesseeViewModel model, User user)
        {
            Lessee originalLessee = context.Lessees.Find(id);
            if (model.AddHistoryInd)
            {
                LesseeHistory lesseeHistory = new LesseeHistory
                {
                    Id = 0,
                    LesseeId = id,
                    LesseeName = originalLessee.LesseeName,
                    CreatedByFirstName = user.NameFirst,
                    CreatedByLastName = user.NameLast,
                    CreatedBy = user.Id,
                    UpdatedBy = user.Id,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                };
                context.LesseeHistories.Add(lesseeHistory);
            }
            originalLessee.LesseeName = model.LesseeName;
            context.SaveChanges();
            return originalLessee;
        }
    }
}
