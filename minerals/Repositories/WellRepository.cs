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
    public class WellRepository : IWellRepository
    {
        private readonly DataContext context;

        public WellRepository(DataContext ctx) => context = ctx;

        public async Task<IEnumerable<Well>> GetAllWellsByPadAsync(long Id) => await context.Wells.Where(x => x.PadId == Id).ToListAsync().ConfigureAwait(false);

        public WellViewModel GetWellById(long id) =>
            context.Wells
            .Include(x => x.Pad)
            .ThenInclude(x => x.Tract)
            .Where(x => x.Id == id)
                .Select(x => new WellViewModel
                {
                    WellId = x.Id,
                    WellNum = x.WellNum,
                    ApiNum = x.ApiNum,
                    PadId = x.Pad.Id,
                    Elevation = x.Elevation,
                    GLElevation = x.GLElevation,
                    LogStartDepth = x.LogStartDepth,
                    LogEndDepth = x.LogEndDepth,
                    LogNotes = x.LogNotes,
                    AltId = x.AltId,
                    Contractid = x.ContractId,
                    HDepth = x.HDepth,
                    VDepth = x.VDepth,
                    SpudDate = x.SpudDate,
                    InitialProductionDate = x.InitialProductionDate,
                    BofAppDate = x.BofAppDate,
                    PlugDate = x.PlugDate,
                    ShutInDate = x.ShutInDate,
                    CompletionDate = x.CompletionDate,
                    Lat = x.Lat,
                    Long = x.Long,
                    AltIdType = x.AltIdType ?? Constants.AltIdTypeNA,
                    BelowGroundInd = x.BelowGroundInd,
                    PrivatePropertyInd = x.PrivatePropertyInd,
                    PadName = x.Pad.PadName,
                    AutoUpdatedAllowedInd = x.AutoUpdatedAllowedInd,
                    TractId = x.Pad.Tract.Id,
                    TractNum = x.Pad.Tract.TractNum,
                    WellStatusId = x.WellStatusId,
                    LesseeId = x.LesseeId,
                    WellTypeId = x.WellTypeId,
                    TownshipId = x.TownshipId,
                    DeepestFormationId = x.DeepestFormationId,
                    ProducingFormationId = x.ProducingFormationId,
                    AcreageAttributableToWells = x.AcreageAttributableToWells,
                    WellboreLengthInd = x.WellboreLengthInd,
                    TotalBoreholeLength = x.TotalBoreholeLength,
                    TotalBoreholeLengthOverrideInd = x.TotalBoreholeLengthOverrideInd
                })
                .FirstOrDefault();

        public IEnumerable<WellDataTableViewModel> GetWellsForDataTable() {

            List<WellDataTableViewModel> retval = new List<WellDataTableViewModel>();
            List<Tract> tracts = context.Tracts.ToList();
            List <Well> wells = context.Wells
              .Include(x => x.WellStatus)
              .Include(x => x.Lessee)
              .Include(x => x.WellTractInformations)
              .ToList();

            foreach (var well in wells)
            {
               
                    retval.Add(new WellDataTableViewModel
                    {
                        WellId = well.Id,
                        WellNum = well.WellNum,
                        ApiNum = well.ApiNum,
                        AltId = well.AltId,
                        LesseeName = well?.Lessee?.LesseeName,
                        Status = well.WellStatus.WellStatusName,
                        TractNum = getTractNums(well.WellTractInformations.Where(x => x.ActiveInd == true && x.TractId != null), tracts )
                    });
                 
            }
            return retval;

            }

        private string getTractNums(IEnumerable<WellTractInformation> wellTractInformations, List<Tract> tracts)
            => string.Join(", ",
                tracts.Where(x => wellTractInformations.Select(y => y.TractId.Value).Distinct().Contains(x.Id))
                .Select(x => x.Administrative == true ? $"{x.TractNum}(admin)" : x.TractNum).ToArray());
        

        public void Update(WellViewModel model, long userId)
        {
            var well = context.Wells.Find(model.WellId);
            well.PadId = model.WellTractInfos.ToArray()[0].PadId;
            well.WellNum = model.WellNum;
            well.ApiNum = model.ApiNum;
            well.Elevation = model.Elevation;
            well.GLElevation = model.GLElevation;
            well.LogStartDepth = model.LogStartDepth;
            well.LogEndDepth = model.LogEndDepth;
            well.LogNotes = model.LogNotes;
            well.AltId = model.AltId;
            well.ContractId = model.Contractid;
            well.AltIdType = model.AltIdType;
            well.HDepth = model.HDepth;
            well.VDepth = model.VDepth;
            well.SpudDate = model.SpudDate;
            well.InitialProductionDate = model.InitialProductionDate;
            well.PlugDate = model.PlugDate;
            well.BofAppDate = model.BofAppDate;
            well.ShutInDate = model.ShutInDate;
            well.CompletionDate = model.CompletionDate;
            well.Lat = model.Lat;
            well.Long = model.Long;
            well.TownshipId = model.TownshipId;
            well.BelowGroundInd = model.BelowGroundInd;
            well.PrivatePropertyInd = model.PrivatePropertyInd;
            well.LesseeId = model.LesseeId;
            well.WellStatusId = model.WellStatusId;
            well.WellTypeId = model.WellTypeId;
            well.AutoUpdatedAllowedInd = model.AutoUpdatedAllowedInd;
            well.ProducingFormationId = model.ProducingFormationId;
            well.DeepestFormationId = model.DeepestFormationId;
            well.AcreageAttributableToWells = model.AcreageAttributableToWells;
            well.WellboreLengthInd = model.WellboreLengthInd;
            well.TotalBoreholeLength = model.TotalBoreholeLength;
            well.TotalBoreholeLengthOverrideInd = model.TotalBoreholeLengthOverrideInd;
            well.LastUpdateDate = DateTime.Now;
            well.UpdatedBy = userId;
            context.SaveChanges();
        }

        public object UpdateAllAutoUpdatedAllowedInd(WellViewModel model)
        {
            long[] wellIds = context.WellTractInformations
                .Where(x => x.ActiveInd == true && x.ContractId == model.Contractid)
                .Select(x => x.WellId)
                .Distinct()
                .ToArray();

            var wells = context.Wells.Where(x => wellIds.Contains(x.Id));
            foreach (var well in wells)
            {
                well.AutoUpdatedAllowedInd = model.AutoUpdatedAllowedInd;
            }
            context.SaveChanges();
            return wells.ToList();
            
        }

        public WellViewModel UpdateAutoUpdatedAllowedInd(WellViewModel model)
        {
            var well = context.Wells.Find(model.WellId);
            well.AutoUpdatedAllowedInd = !well.AutoUpdatedAllowedInd;
            context.SaveChanges();
            return new WellViewModel(){ WellId = model.WellId, AutoUpdatedAllowedInd = well.AutoUpdatedAllowedInd};
        }
    }
}
