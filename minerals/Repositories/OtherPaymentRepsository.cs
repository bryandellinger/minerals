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
    public class OtherPaymentRepsository : IOtherPaymentRepository
    {
        private readonly DataContext context;
        public OtherPaymentRepsository(DataContext ctx) => context = ctx;
        public async Task<object> GetAllAsync() =>
            await (from otherRentals in context.OtherRentals
                   join checks in context.Checks on otherRentals.CheckId equals checks.Id
                   join lessees in context.Lessees on checks.LesseeId equals lessees.Id
                   select new {
                       otherRentals.Id,
                       otherRentals.OtherPaymentType,
                       otherRentals.OtherRentalEntryDate,
                       otherRentals.OtherRentPay,
                       otherRentals.OtherRentalNotes,
                       checks.CheckNum,
                       checks.ReceivedDate,
                       checks.CheckDate,
                       lessees.LesseeName
                   })
            .ToListAsync()
            .ConfigureAwait(false);

        public  async Task<object> GetByCheckAsync(long id) =>
                 await (from otherRentals in context.OtherRentals
                        join checks in context.Checks.Where(x => x.Id == id) on otherRentals.CheckId equals checks.Id
                        join lessees in context.Lessees on checks.LesseeId equals lessees.Id
                        select new
                        {
                            otherRentals.Id,
                            otherRentals.OtherPaymentType,
                            otherRentals.OtherRentalEntryDate,
                            otherRentals.OtherRentPay,
                            otherRentals.OtherRentalNotes,
                            checks.CheckNum,
                            checks.ReceivedDate,
                            checks.CheckDate,
                            lessees.LesseeName
                        })
            .ToListAsync()
            .ConfigureAwait(false);

        public void Update(OtherRental model, long userId)
        {
            var otherRental = context.OtherRentals.Find(model.Id);
            otherRental.OtherRentalNotes = model.OtherRentalNotes;
            otherRental.CheckId = model.CheckId;
            otherRental.OtherPaymentType = model.OtherPaymentType;
            otherRental.OtherRentPay = model.OtherRentPay;
            otherRental.OtherRentalEntryDate = model.OtherRentalEntryDate;
            otherRental.LastUpdateDate = DateTime.Now;
            otherRental.UpdatedBy = userId;
            context.SaveChanges();
        }
    }
}
