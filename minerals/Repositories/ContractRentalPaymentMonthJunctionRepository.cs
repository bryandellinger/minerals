using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class ContractRentalPaymentMonthJunctionRepository : IContractRentalPaymentMonthJunctionRepository
    {
        private readonly DataContext context;
        public ContractRentalPaymentMonthJunctionRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetContractRentalPaymentMonthsByContractAsync(long id) =>
          await (from contracts in context.Contracts.Where(x => x.Id == id)
                 join contractRentalPaymentMonthJunctions in context.ContractRentalPaymentMonthJunctions on contracts.Id equals contractRentalPaymentMonthJunctions.ContractId
                 join months in context.Months on contractRentalPaymentMonthJunctions.MonthId equals months.Id
                 select new
                 {
                     months.Id
                 })
                    .ToListAsync()
                    .ConfigureAwait(false);

        public async Task<object> GetContractRentalPaymentMonthsByContractRentalAsync(long id) =>
        await(from contractRentals in context.ContractRentals.Where(x => x.Id == id)
           join contractRentalPaymentMonthJunctions in context.ContractRentalPaymentMonthJunctions on contractRentals.ContractId equals contractRentalPaymentMonthJunctions.ContractId
              join months in context.Months on contractRentalPaymentMonthJunctions.MonthId equals months.Id
           select new
            {
                months.Id
            })
                    .ToListAsync()
                    .ConfigureAwait(false);
      }
    }

