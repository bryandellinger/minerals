using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Minerals.BusinessLogic
{
    public class UnitBusinessLogic : IUnitBusinessLogic
    {
        private IGenericRepository<Unit> genericUnitRepository;
        private IUnitRepository unitRepository;
        private IGenericRepository<TractUnitJunction> tractUnitJunctionRepository;
        private IGenericRepository<File> fileRepository;
        private IGenericRepository<WellTractInformation> genericWellTractInfoRepository;
        private IGenericRepository<TractUnitJunctionWellJunction> genericTractUnitJunctionWellJunctionRepository;
        private IContractRepository contractRepository;

        public UnitBusinessLogic(
            IGenericRepository<Unit> genericUnitRepo,
            IUnitRepository unitRepo,
            IGenericRepository<TractUnitJunction> tractUnitJunctionRepo,
            IGenericRepository<File> fileRepo,
            IGenericRepository<WellTractInformation> genericWellTractInfoRepo,
            IGenericRepository<TractUnitJunctionWellJunction> genericTractUnitJunctionWellJunctionRepo,
            IContractRepository contactRepo
            )
        {
            genericUnitRepository = genericUnitRepo;
            unitRepository = unitRepo;
            tractUnitJunctionRepository = tractUnitJunctionRepo;
            fileRepository = fileRepo;
            genericWellTractInfoRepository = genericWellTractInfoRepo;
            genericTractUnitJunctionWellJunctionRepository = genericTractUnitJunctionWellJunctionRepo;
            contractRepository = contactRepo;
        }

        public object Save(UnitViewModel model, long userId)
        {
            if (model.Id > 0)
            {
                if (model.IsActiveInd && model.AmendmentInd)
                {
                    unitRepository.AddAmendment(model, userId);
                }
                unitRepository.Update(model, userId);
                updateTractUnitJunctions(model.Id, model.TractUnitJunctions);
                updateFiles(model.Id, model.Files);
                UpdateWellTractInformation(model.Id);
                return new Unit { Id = model.Id };
            }
            else
            {
                List<TractUnitJunction> tractUnitJunctions = new List<TractUnitJunction>();
                foreach (var item in model.TractUnitJunctions)
                {
                    List<TractUnitJunctionWellJunction> tractUnitJunctionWellJunctions = new List<TractUnitJunctionWellJunction>();

                    foreach (var junction in item.TractUnitJunctionWellJunctions)
                    {
                        tractUnitJunctionWellJunctions.Add(new TractUnitJunctionWellJunction
                        {
                            WellId = junction.WellId
                        });
                    }

                    tractUnitJunctions.Add(new TractUnitJunction
                    {
                        TractId = item.TractId,
                        COPAcres = item.COPAcres,
                        TractUnitJunctionWellJunctions = tractUnitJunctionWellJunctions
                    }); ;
                }

                List<File> files = new List<File>();
                foreach (var item in model.Files)
                {
                    files.Add(new File { FileGuid = item.FileGuid,
                        FileExtension = item.FileExtension,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        FileIcon = item.FileIcon
                    });
                    fileRepository.Delete(item.Id);
                }

                Unit newUnit = genericUnitRepository.Insert(new Unit
                {
                    Id = 0,
                    UnitName = model.UnitName,
                    AlternateId = model.AlternateId,
                    GISAcres = model.GISAcres,
                    DPUAcres = model.DPUAcres,
                    IsActiveInd = true,
                    UnitGroup = new UnitGroup { Id = 0},
                    DPUAcresEffectiveDate = model.DPUAcresEffectiveDate,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    TractUnitJunctions = tractUnitJunctions,
                    Files = files
                });
                UpdateWellTractInformation(newUnit.Id);
                return new Unit { Id = newUnit.Id };
            }
        }

        private void UpdateWellTractInformation(long id)
        {
            // only update if this unit is active (most recent amendment)
            Unit unit = genericUnitRepository.GetById(id);
            if (unit.IsActiveInd)
            {
                // get all well ownership information
                List<WellTractInformation> wellTractInformations = genericWellTractInfoRepository.GetAll().ToList();

                // get all the tracts related to the unit
                List<Tract> tracts = unitRepository.GetTractsByUnit(id);

                // get an array of the tract ids
                long[] unitTractIds = tracts.Select((x) => x.Id).ToArray();

                // get all the wells related to the unit
                List<Well> wells = unitRepository.GetWellsByUnit(id);

                foreach (Well well in wells)
                {
                    // find the well tract information table for that well
                    List<WellTractInformation> filteredWellTractInformatinos = 
                        wellTractInformations.Where(x => x.WellId == well.Id && x.ActiveInd).ToList();

                    // get its tract ids
                    long[] wtiTractIds = filteredWellTractInformatinos.Where(x => x.TractId != null).Select((x) => x.TractId.Value).ToArray();

                    // get the tractids that are in units but not in well tract info
                    long[] tractIdsToAdd = unitTractIds.Except(wtiTractIds).ToArray();

                    // loop through the tracts if the well tract information does not have the tract then add it.
                    foreach (long tractId in tractIdsToAdd)
                    {
                        Contract contract = contractRepository.getContractByTract(tractId);

                        foreach (var contractEventDetail in contract.ContractEventDetails.Where(x => x.ActiveInd))
                        {
                            var retval = genericWellTractInfoRepository.Insert(new WellTractInformation
                            {
                                Id = 0,
                                ActiveInd = true,
                                PercentOwnership = contractEventDetail.ShareOfLeasePercentage,
                                RoyaltyPercent = contractEventDetail.RoyaltyPercent,
                                TractId = tractId,
                                LesseeId = contractEventDetail.LesseeId,
                                LesseeName = contractEventDetail.LesseeName,
                                WellId = well.Id,
                                ContractId = contract.Id,                              
                            });
                        }


                    }

                }
            }
           
        }

        private void updateFiles(long id, IEnumerable<File> files)
        {
            foreach (var item in files)
            {
                fileRepository.Delete(item.Id);             
            }
            unitRepository.UpdateFiles(id, files);
        }

        private void updateTractUnitJunctions(long id, IEnumerable<TractUnitJunction> tractUnitJunctions)
        {
            foreach (var item in tractUnitJunctionRepository.GetAll().Where(x => x.UnitId == id))
            {
                tractUnitJunctionRepository.Delete(item.Id);
            }

            foreach (var item in tractUnitJunctions)
            {
                List<TractUnitJunctionWellJunction> tractUnitJunctionWellJunctions = new List<TractUnitJunctionWellJunction>();

                foreach (var junction in item.TractUnitJunctionWellJunctions)
                {
                    tractUnitJunctionWellJunctions.Add(new TractUnitJunctionWellJunction
                    {
                        WellId = junction.WellId
                    });
                }

                tractUnitJunctionRepository.Insert(new TractUnitJunction
                {
                    Id = 0,
                    UnitId = id,
                    COPAcres = item.COPAcres,
                    TractId = item.TractId,
                    TractUnitJunctionWellJunctions = tractUnitJunctionWellJunctions
                });

            }

        }
    }
}
