using Microsoft.AspNetCore.Http;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace Minerals.BusinessLogic
{
    public class WellBusinessLogic : IWellBusinessLogic
    {
        private readonly IGenericRepository<Pad> padRepository;
        private readonly IGenericRepository<Well> genericWellRepository;
        private readonly IWellRepository wellRepository;
        private readonly IGenericRepository<WellTractInformation> genericWellTractInformationRepository;
        private readonly IWellTractInformationRepository wellTractInformationRepository;
        private readonly IGenericRepository<Lessee> lesseeRepository;
        private readonly IGenericRepository<DigitalWellLogTestTypeWellJunction> digitalWellLogTestTypeWellRepository;
        private readonly IGenericRepository<DigitalImageWellLogTestTypeWellJunction> digitalImageWellLogTestTypeWellRepository;
        private readonly IGenericRepository<HardCopyWellLogTestTypeWellJunction> hardCopyWellLogTestTypeWellRepository;
        private readonly IGenericRepository<WellboreShare> wellboreShareRepository;
        public WellBusinessLogic(
            IGenericRepository<Pad> padRepo,
            IGenericRepository<Well> genericWellRepo,
            IWellRepository wellRepo,
            IGenericRepository<WellTractInformation> genericWellTractInformationRepo,
            IWellTractInformationRepository wellTractInformationRepo,
            IGenericRepository<Lessee> leseeRepo,
            IGenericRepository<DigitalWellLogTestTypeWellJunction> digitalWellLogTestTypeWellRepo,
            IGenericRepository<DigitalImageWellLogTestTypeWellJunction> digitalImageWellLogTestTypeWellRepo,
            IGenericRepository<HardCopyWellLogTestTypeWellJunction> hardCopyWellLogTestTypeWellRepo,
            IGenericRepository<WellboreShare> wellboreShareRepo
            )
        {
            padRepository = padRepo;
            genericWellRepository = genericWellRepo;
            wellRepository = wellRepo;
            genericWellTractInformationRepository = genericWellTractInformationRepo;
            wellTractInformationRepository = wellTractInformationRepo;
            lesseeRepository = leseeRepo;
            digitalWellLogTestTypeWellRepository = digitalWellLogTestTypeWellRepo;
            digitalImageWellLogTestTypeWellRepository = digitalImageWellLogTestTypeWellRepo;
            hardCopyWellLogTestTypeWellRepository = hardCopyWellLogTestTypeWellRepo;
            wellboreShareRepository = wellboreShareRepo;
        }
        public WellViewModel Save(WellViewModel model, long userId)
        {
            /// create pads if it does not exist.
            foreach (var item in model.WellTractInfos)
            {
                
                if (!string.IsNullOrEmpty(item.PadName) && !string.IsNullOrWhiteSpace(item.PadName))
                {
                    if (item.PadId < 1)
                    {
                        var pad = padRepository.GetAll().FirstOrDefault(x => x.PadName == item.PadName && x.TractId == item.TractId.Value);
                        if (pad == null)
                        {
                            item.PadId = padRepository.Insert(new Pad { Id = 0, PadName = item.PadName, TractId = item.TractId.Value }).Id;
                        }
                        else
                        {
                            item.PadId = pad.Id;
                        }

                    }
                }
            }
                        
            if (model.WellId < 1)
            {
  
               
                List<WellTractInformation> wellTractInformations = new List<WellTractInformation>();
                List<Lessee> lessees = lesseeRepository.GetAll().ToList();
                List<WellboreShare> wellboreShares = new List<WellboreShare>();
         
                foreach (var item in model.WellTractInfos)
                {
                    string lesseeName = null;
                    if (item.LesseeId != null)
                    {
                        var lessee = lessees.FirstOrDefault(x => x.Id == item.LesseeId.Value);
                        if (lessee != null)
                        {
                            lesseeName = lessee.LesseeName;
                        }

                    }

                    if (string.IsNullOrEmpty(item.PadName) || string.IsNullOrWhiteSpace(item.PadName))
                    {
                        wellTractInformations.Add(new WellTractInformation
                        {
                            TractId = item.TractId,
                            ContractId = item.ContractId,
                            LesseeId = item.LesseeId,
                            PercentOwnership = item.PercentOwnership,
                            RoyaltyPercent = item.RoyaltyPercent,
                            ActiveInd = true,
                            LesseeName = lesseeName,
                        });
                    }
                    else
                    {
                        wellTractInformations.Add(new WellTractInformation
                        {
                            TractId = item.TractId,
                            PadId = item.PadId,
                            ContractId = item.ContractId,
                            LesseeId = item.LesseeId,
                            PercentOwnership = item.PercentOwnership,
                            RoyaltyPercent = item.RoyaltyPercent,
                            ActiveInd = true,
                            LesseeName = lesseeName,
                        });
                    }

                }

                foreach (var item in model.wellboreShares)
                {
                    wellboreShares.Add(new WellboreShare { UnitId = item.UnitId, LengthInUnit = item.LengthInUnit });
                }
                Well newWell = genericWellRepository.Insert(new Well
                {
                    Id = 0,
                    PadId = model.WellTractInfos.ToArray()[0].PadId,
                    WellNum = model.WellNum,
                    ApiNum = model.ApiNum,
                    LesseeId = model.LesseeId,
                    AltId = model.AltId,
                    ContractId = model.Contractid,
                    AltIdType = model.AltIdType,
                    TownshipId = model.TownshipId,
                    Elevation = model.Elevation,
                    GLElevation = model.GLElevation,
                    LogStartDepth = model.LogStartDepth,
                    LogEndDepth = model.LogEndDepth,
                    LogNotes = model.LogNotes,
                    HDepth = model.HDepth,
                    VDepth = model.VDepth,
                    SpudDate = model.SpudDate,
                    InitialProductionDate = model.InitialProductionDate,
                    BofAppDate = model.BofAppDate,
                    PlugDate = model.PlugDate,
                    ShutInDate = model.ShutInDate,
                    CompletionDate = model.CompletionDate,
                    AutoUpdatedAllowedInd = model.AutoUpdatedAllowedInd,
                    Lat = model.Lat,
                    Long = model.Long,
                    BelowGroundInd = model.BelowGroundInd,
                    PrivatePropertyInd = model.PrivatePropertyInd,
                    WellStatusId = model.WellStatusId,
                    WellTypeId = model.WellTypeId,
                    ProducingFormationId = model.ProducingFormationId,
                    DeepestFormationId = model.DeepestFormationId,
                    AcreageAttributableToWells = model.AcreageAttributableToWells,
                    WellboreLengthInd = model.WellboreLengthInd,
                    TotalBoreholeLength = model.TotalBoreholeLength,
                    TotalBoreholeLengthOverrideInd = model.TotalBoreholeLengthOverrideInd,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    WellTractInformations = wellTractInformations,
                    WellboreShares = wellboreShares
                });
               
                var insertedWell =  wellRepository.GetWellById(newWell.Id);
                updateTestLogs(insertedWell.WellId, model.DigitalLogs, model.DigitalImageLogs, model.HardCopyLogs);
                return insertedWell;

            }
            else
            {
                wellRepository.Update(model, userId);
                updateWellTractInformation(model.WellId, model.WellTractInfos, model.ContractEventDetailReasonsForChangeId);
                updateTestLogs(model.WellId, model.DigitalLogs, model.DigitalImageLogs, model.HardCopyLogs);
                updateWellboreShares(model.WellId, model.wellboreShares);
                return wellRepository.GetWellById(model.WellId);
            }
    
        }

        private void updateWellboreShares(long wellId, IEnumerable<WellboreShare> wellboreShares)
        {
            foreach (var item in wellboreShareRepository.GetAll().Where(x => x.WellId == wellId))
            {
                wellboreShareRepository.Delete(item.Id);
            }
            foreach (var item in wellboreShares)
            {
                wellboreShareRepository.Insert(new WellboreShare { Id = 0, WellId = wellId, UnitId = item.UnitId, LengthInUnit = item.LengthInUnit });
            }
        }

        private void updateTestLogs(long wellId, IEnumerable<long> digitalLogs, IEnumerable<long> digitalImageLogs, IEnumerable<long> hardCopyLogs)
        {
            foreach (var item in digitalImageWellLogTestTypeWellRepository.GetAll().Where(x => x.WellId == wellId))
            {
                digitalImageWellLogTestTypeWellRepository.Delete(item.Id);
            }
            foreach (var item in digitalWellLogTestTypeWellRepository.GetAll().Where(x => x.WellId == wellId))
            {
                digitalWellLogTestTypeWellRepository.Delete(item.Id);
            }
            foreach (var item in hardCopyWellLogTestTypeWellRepository.GetAll().Where(x => x.WellId == wellId))
            {
                hardCopyWellLogTestTypeWellRepository.Delete(item.Id);
            }
            foreach (var item in digitalLogs)
            {
                digitalWellLogTestTypeWellRepository.Insert(new DigitalWellLogTestTypeWellJunction { Id = 0, WellId = wellId, WellLogTestTypeId = item });
            }
            foreach (var item in digitalImageLogs)
            {
                digitalImageWellLogTestTypeWellRepository.Insert(new DigitalImageWellLogTestTypeWellJunction { Id = 0, WellId = wellId, WellLogTestTypeId = item });
            }
            foreach (var item in hardCopyLogs)
            {
                hardCopyWellLogTestTypeWellRepository.Insert(new HardCopyWellLogTestTypeWellJunction { Id = 0, WellId = wellId, WellLogTestTypeId = item });
            }
        }

        private void updateWellTractInformation(long wellId, IEnumerable<WellTractInformationViewModel> wellTractInfos, long? contractEventDetailReasonsForChangeId)
        {

            if (contractEventDetailReasonsForChangeId != null)
            {
                wellTractInformationRepository.SetActiveRecordsToHistoric(wellId, contractEventDetailReasonsForChangeId, DateTime.Now);
            }

            if (!genericWellTractInformationRepository.GetAll().Any(x => x.WellId == wellId && x.ActiveInd == true))
             {
                List<Lessee> lessees = lesseeRepository.GetAll().ToList();
                foreach (var item in wellTractInfos)
                {
                    string lesseeName = null;
                    if (item.LesseeId != null)
                    {
                        var lessee = lessees.FirstOrDefault(x => x.Id == item.LesseeId.Value);
                        if (lessee != null)
                        {
                            lesseeName = lessee.LesseeName;
                        }

                    }
                    var retval = genericWellTractInformationRepository.Insert(new WellTractInformation
                    {
                        TractId = item.TractId,
                        PadId = item.PadId,
                        WellId = wellId,
                        ContractId = item.ContractId,
                        LesseeId = item.LesseeId,
                        PercentOwnership = item.PercentOwnership,
                        RoyaltyPercent = item.RoyaltyPercent,
                        ActiveInd = true,
                        LesseeName = lesseeName
                    });
                }
            }

        }
    

    }
}
