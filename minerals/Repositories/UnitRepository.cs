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
    public class UnitRepository : IUnitRepository
    {
        private readonly DataContext context;

        public UnitRepository(DataContext ctx) => context = ctx;

        public void AddAmendment(UnitViewModel model, long userId)
        {
            Unit unit = context.Units
                .Include(x => x.TractUnitJunctions)
                .Include(x => x.Files)
                .First(x => x.Id == model.Id);

            List<TractUnitJunctionWellJunction> tractUnitJuctionWellJunctions = new List<TractUnitJunctionWellJunction>();
            List<TractUnitJunction> tractUnitJunctions = new List<TractUnitJunction>();
            List<File> files = new List<File>();

            foreach (var tuj in unit.TractUnitJunctions)
            {
                foreach (var tujwj in context.TractUnitJunctionWellJunctions.Where(x => x.TractUnitJunctionId == tuj.Id).ToList())
                {
                    tractUnitJuctionWellJunctions.Add(new TractUnitJunctionWellJunction { WellId = tujwj.WellId });
                }

                tractUnitJunctions.Add(new TractUnitJunction { TractId = tuj.TractId, COPAcres = tuj.COPAcres, TractUnitJunctionWellJunctions = tractUnitJuctionWellJunctions });
            }

            foreach (File file in unit.Files)
            {
                files.Add(new File
                {
                    FileExtension = file.FileExtension,
                    FileGuid = file.FileGuid,
                    FileIcon = file.FileIcon,
                    FileName = file.FileName,
                    FileSize = file.FileSize                  
                });
            }

            Unit newUnit = new Unit
            {
                Id = 0,
                UnitName = unit.UnitName,
                AmendmentName = unit.AmendmentName,
                AlternateId = unit.AlternateId,
                GISAcres = unit.GISAcres,
                DPUAcres = unit.DPUAcres,
                IsActiveInd = false,
                UnitGroupId = unit.UnitGroupId,
                DPUAcresEffectiveDate = unit.DPUAcresEffectiveDate,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                CreatedBy = userId,
                UpdatedBy = userId,
                TractUnitJunctions = tractUnitJunctions,
                Files = files
            };

            context.Units.Add(newUnit);

        }

        public List<Tract> GetTractsByUnit(long id) =>

            (from u in context.Units.Where(x => x.Id == id && x.IsActiveInd)
             join tuj in context.TractUnitJunctions on u.Id equals tuj.UnitId
             join t in context.Tracts on tuj.TractId equals t.Id
             select t).ToList();
        

        public  async Task<object> GetUnitGroupInfoByUnit(long id) =>
           await(from u1 in context.Units.Where(x => x.Id == id)
                  join ug  in context.UnitGroups on u1.UnitGroupId equals ug.Id
                  join u2 in context.Units on ug.Id equals u2.UnitGroupId
                  select new
                  {
                      u2.Id,
                      u2.UnitName,
                      u2.AmendmentName,
                      u2.IsActiveInd,
                      u2.CreateDate
                  })
                   .OrderByDescending(x => x.IsActiveInd)
                    .ThenByDescending(x => x.CreateDate)
                  .ToListAsync().ConfigureAwait(false);



        public async Task<object> GetUnitsForDataTableAsync()
        {
            double? copAcres = null;
            var List1 = await context.Units
             .Where(x => !x.TractUnitJunctions.Any() && x.IsActiveInd == true)
              .Select(x => new
              {
                  x.Id,
                  x.UnitName,
                  x.DPUAcres,
                  COPAcres = copAcres,
                  TractNum = String.Empty,
                  Wells = String.Empty,
                  NumOfWells = 0,
              })
             .ToListAsync().ConfigureAwait(false);

            var List2 = await context.TractUnitJunctions.Include(x => x.Unit).Where(x => x.Unit.IsActiveInd == true)
                 .Select(x => new
                 {
                     Id = x.UnitId,
                     x.Unit.UnitName,
                     x.Unit.DPUAcres,
                     x.COPAcres,
                     x.Tract.TractNum,
                     Wells = string.Join(", ", x.TractUnitJunctionWellJunctions
                                              .Select(z => $"{z.Well.WellNum} ({z.Well.ApiNum})")
                                        ),
                     NumOfWells = x.TractUnitJunctionWellJunctions.Count()
                 })
                .ToListAsync().ConfigureAwait(false);


            return List1.Concat(List2);
        }

        public List<Well> GetWellsByUnit(long id) =>
           (from u in context.Units.Where(x => x.Id == id && x.IsActiveInd)
            join tuj in context.TractUnitJunctions on u.Id equals tuj.UnitId
            join tujwj in context.TractUnitJunctionWellJunctions on tuj.Id equals tujwj.TractUnitJunctionId
            join w in context.Wells on tujwj.WellId equals w.Id
            select w).Distinct().ToList();

        public void Update(UnitViewModel model, long userId)
        {
            var unit = context.Units.Find(model.Id);

            unit.UnitName = model.UnitName;
            unit.AmendmentName = model.AmendmentName;
            unit.AlternateId = model.AlternateId;
            unit.GISAcres = model.GISAcres;
            unit.DPUAcres = model.DPUAcres;
            unit.DPUAcresEffectiveDate = model.DPUAcresEffectiveDate;
            unit.LastUpdateDate = DateTime.Now;
            unit.UpdatedBy = userId;

            context.SaveChanges();
        }

        public void UpdateFiles(long id, IEnumerable<File> files)
        {
            List<File> unitFiles = context.Units.Include(x => x.Files).FirstOrDefault(x => x.Id == id).Files.ToList();
            foreach (var file in unitFiles)
            {
                context.Files.Remove(file);
                context.SaveChanges();
            }

            var unit2 = context.Units.Find(id);

            List<File> newFiles = new List<File>();
            foreach (var item in files)
            {
                newFiles.Add(new File { FileExtension = item.FileExtension,
                    FileName = item.FileName,
                    FileGuid = item.FileGuid,
                    FileSize = item.FileSize,
                    FileIcon = item.FileIcon});
            }
            unit2.Files = newFiles;
            context.SaveChanges();

        }
    }
}
