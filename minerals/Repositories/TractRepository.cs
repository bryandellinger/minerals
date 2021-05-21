using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class TractRepository : ITractRepository
    {
        private readonly DataContext context;
        public TractRepository(DataContext ctx) => context = ctx;
        public async Task<IEnumerable<Tract>> GetAllAsync() => await Query().OrderBy(p => p.TractNum).ToListAsync().ConfigureAwait(false);

        public async Task<Tract> GetByIdAsync(long id) => await Query().FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

        public Tract Update(Tract model)
        {
            var tract = context.Tracts.Find(model.Id);
            tract.TractNum = model.TractNum;
            tract.Terminated = model.Terminated;
            tract.TerminatedDate = model.TerminatedDate;
            tract.Administrative = model.Administrative;
            tract.Acreage = model.Acreage;
            tract.ReversionDate = model.ReversionDate;
            tract.AltTractNum = model.AltTractNum;
            var districtTractJunctions = context.DistrictTractJunctions.Where(x => x.TractId == model.Id);
            context.RemoveRange(districtTractJunctions);
            foreach (var item in model.DistrictTracts)
            {
                context.DistrictTractJunctions.Add(new DistrictTractJunction { Id = 0, TractId = model.Id, DistrictId = item.DistrictId });
            }
            context.SaveChanges();
            return model;
        }

        public Tract UpdateAdminTract(long id)
        {
            var tract = context.Tracts.Find(id);
            tract.Administrative = false;
            context.SaveChanges();
            return tract;
        }

        private IQueryable<Tract> Query() => context.Tracts
      .Select(p => new Tract
      {
          Id = p.Id,
          Acreage = p.Acreage,
          Administrative = p.Administrative,
          BondsRequired = p.BondsRequired,
          Horizon = p.Horizon,
          IncreasedRentAmnt = p.IncreasedRentAmnt,
          LandIncluded = p.LandIncluded,
          LeaseDate = p.LeaseDate,
          Notes = p.Notes,
          NotesAdditional = p.NotesAdditional,
          Rent = p.Rent,
          RentalAnnivDate = p.RentalAnnivDate,
          RentalMonthDue = p.RentalMonthDue,
          RentalsRequired = p.RentalsRequired,
          RentAsOfToday = p.RentAsOfToday,
          ReversionDate = p.ReversionDate,
          RiverTract = p.RiverTract,
          RoyaltyFlDollar = p.RoyaltyFlDollar,
          RoyaltyFloor = p.RoyaltyFloor,
          RoyaltyPercent = p.RoyaltyPercent,
          StateParkName = p.StateParkName,
          Terminated = p.Terminated,
          TerminatedDate = p.TerminatedDate,
          TractNum = p.TractNum,
          YearIncRentEffec = p.YearIncRentEffec,
          AltTractNum = p.AltTractNum,
          LesseeId = p.LesseeId,
      });

    }
}
