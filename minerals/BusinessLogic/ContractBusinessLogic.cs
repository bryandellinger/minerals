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
    public class ContractBusinessLogic : IContractBusinessLogic
    {
        private readonly IGenericRepository<Tract> genericTractRepository;
        private readonly ITractRepository tractRepository;
        private readonly IGenericRepository<DistrictContractJunction> districtContractJunctionRepository;
        private readonly IGenericRepository<LandLeaseAgreement> landLeaseAgreementRepository;
        private readonly IGenericRepository<ContractType> contractTypeRepository;
        private readonly IGenericRepository<ContractSubType> contractSubTypeRepository;
        private readonly IGenericRepository<TownshipLandLeaseAgreementJunction> townshipLandLeaseAgreementJunctionRepository;
        private readonly IGenericRepository<TownshipSurfaceUseAgreementJunction> townshipSurfaceUseAgreementJunctionRepository;
        private readonly IGenericRepository<TownshipProspectingAgreementJunction> townshipProspectingAgreementJunctionRepository;
        private readonly IGenericRepository<TownshipProductionAgreementJunction> townshipProductionAgreementJunctionRepository;
        private readonly IGenericRepository<TownshipSeismicAgreementJunction> townshipSeismicAgreementJunctionRepository;
        private readonly IGenericRepository<RowContract> rowContractRepository;
        private readonly IGenericRepository<PaymentRequirement> paymentRequirementRepository;
        private readonly IGenericRepository<Contract> contractRepository;
        private readonly IGenericRepository<AssociatedContract> associatedContractRepository;
        private readonly IGenericRepository<AssociatedTract> associatedTractRepository;
        private readonly IGenericRepository<SurfaceUseAgreement> surfaceUseAgreementRepository;
        private readonly IGenericRepository<ProspectingAgreement> prospectingAgreementRepository;
        private readonly IGenericRepository<ProductionAgreement> productionAgreementRepository;
        private readonly IGenericRepository<SeismicAgreement> seismicAgreementRepository;
        private readonly IGenericRepository<ContractEventDetail> genericContractEventDetailRepository;
        private readonly IContractEventDetailRepository contractEventDetailRepository;
        private readonly IGenericRepository<ContractEventDetailReasonForChange> contractEventDetailReasonForChangeRepository;
        private readonly IGenericRepository<Lessee> lesseeRepository;
        private readonly IGenericRepository<Well> wellRepository;
        private readonly IGenericRepository<WellTractInformation> genericWellTractInfoRepository;
        private readonly IWellTractInformationRepository wellTractInfoRepository;
        private readonly IGenericRepository<Pad> padRepository;
        private readonly IGenericRepository<Storage> genericStorageRepository;
        private readonly IStorageRepository storageRepository;
        private readonly IGenericRepository<ContractRentalPaymentMonthJunction> contractRentalPaymentMonthJunctionRepository;
        private readonly CultureInfo ci;

        public ContractBusinessLogic(
            IGenericRepository<Tract> genericTractRepo,
            ITractRepository tractRepo,
            IGenericRepository<DistrictContractJunction> districtContractJunctionRepo,
            IGenericRepository<LandLeaseAgreement> landLeaseAgreementRepo,
            IGenericRepository<ContractType> contractTypeRepo,
            IGenericRepository<ContractSubType> contractSubTypeRepo,
            IGenericRepository<TownshipLandLeaseAgreementJunction> townshipLandLeaseAgreementJunctionRepo,
            IGenericRepository<TownshipSurfaceUseAgreementJunction> townshipSurfaceUseAgreementJunctionRepo,
            IGenericRepository<TownshipProspectingAgreementJunction> townshipProspectingAgreementJunctionRepo,
            IGenericRepository<TownshipProductionAgreementJunction> townshipProductionAgreementJunctionRepo,
            IGenericRepository<TownshipSeismicAgreementJunction> townshipSeismicAgreementJunctionRepo,
            IGenericRepository<RowContract> rowContractRepo,
            IGenericRepository<PaymentRequirement> paymentRequirementRepo,
            IGenericRepository<Contract> contractRepo,
            IGenericRepository<AssociatedContract> associatedContractRepo,
            IGenericRepository<AssociatedTract> associatedTractRepo,
            IGenericRepository<SurfaceUseAgreement> surfaceUseAgreementRepo,
            IGenericRepository<ProspectingAgreement> prospectingAgreementRepo,
            IGenericRepository<ProductionAgreement> productionAgreementRepo,
            IGenericRepository<SeismicAgreement> seismicAgreementRepo,
            IGenericRepository<ContractEventDetail> genericContractEventDetailRepo,
            IContractEventDetailRepository contractEventDetailRepo,
            IGenericRepository<Lessee> lesseeRepo,
            IGenericRepository<ContractEventDetailReasonForChange> contractEventDetailReasonForChangeRepo,
            IGenericRepository<Well> wellRepo,
            IGenericRepository<WellTractInformation> genericWellTractInfoRepo,
            IWellTractInformationRepository wellTractInfoRepo,
            IGenericRepository<Pad> padRepo,
            IGenericRepository<Storage> genericStorageRepo,
            IStorageRepository storageRepo,
            IGenericRepository<ContractRentalPaymentMonthJunction> contractRentalPaymentMonthJunctionRepo

            )
        {
            genericTractRepository = genericTractRepo;
            tractRepository = tractRepo;
            districtContractJunctionRepository = districtContractJunctionRepo;
            landLeaseAgreementRepository = landLeaseAgreementRepo;
            contractTypeRepository = contractTypeRepo;
            contractSubTypeRepository = contractSubTypeRepo;
            townshipLandLeaseAgreementJunctionRepository = townshipLandLeaseAgreementJunctionRepo;
            townshipSurfaceUseAgreementJunctionRepository = townshipSurfaceUseAgreementJunctionRepo;
            townshipProspectingAgreementJunctionRepository = townshipProspectingAgreementJunctionRepo;
            townshipProductionAgreementJunctionRepository = townshipProductionAgreementJunctionRepo;
            townshipSeismicAgreementJunctionRepository = townshipSeismicAgreementJunctionRepo;
            rowContractRepository = rowContractRepo;
            paymentRequirementRepository = paymentRequirementRepo;
            contractRepository = contractRepo;
            associatedContractRepository = associatedContractRepo;
            associatedTractRepository = associatedTractRepo;
            surfaceUseAgreementRepository = surfaceUseAgreementRepo;
            prospectingAgreementRepository = prospectingAgreementRepo;
            productionAgreementRepository = productionAgreementRepo;
            seismicAgreementRepository = seismicAgreementRepo;
            genericContractEventDetailRepository = genericContractEventDetailRepo;
            contractEventDetailRepository = contractEventDetailRepo;
            lesseeRepository = lesseeRepo;
            contractEventDetailReasonForChangeRepository = contractEventDetailReasonForChangeRepo;
            genericWellTractInfoRepository = genericWellTractInfoRepo;
            wellTractInfoRepository = wellTractInfoRepo;
            wellRepository = wellRepo;
            padRepository = padRepo;
            genericStorageRepository = genericStorageRepo;
            storageRepository = storageRepo;
            contractRentalPaymentMonthJunctionRepository = contractRentalPaymentMonthJunctionRepo;
            ci = new CultureInfo("en-US");
        }

        /// <summary> Add the contracts agreement </summary>
        public void AddAgreement(long contractId, AddUpdateContractViewModel model)
        {
            Contract contract = contractRepository.GetById(contractId);
            if (contract != null && contract.ContractTypeId != null)
            {
                ContractType contractType = contractTypeRepository.GetAll().FirstOrDefault(x => x.Id == contract.ContractTypeId);
                if (contractType != null && contractType.ContractTypeName == Constants.ContractTypeLandLease)
                {
                    LandLeaseAgreement landLeaseAgreement = new LandLeaseAgreement
                    {
                        Id = 0,
                        ContractId = contract.Id,
                        EffectiveDate = model.EffectiveDate,
                        ExpirationDate = model.ExpirationDate,
                        BufferAcreage = model.BufferAcreage,
                        NSDAcreage = model.NSDAcreage,
                        NSDAcreageAppliesToAllInd = model.NSDAcreageAppliesToAllInd,
                        InPoolAcreage = model.InPoolAcreage,
                        Acreage = model.Acreage,
                        TerminationDate = model.TerminationDate,
                        TerminationReason = model.TerminationReason,
                        AltIdCategoryId = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdCategoryId : null,
                        AltIdInformation = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdInformation : null,
                        CreatedBy = contract.CreatedBy,
                        UpdatedBy = contract.UpdatedBy,
                        CreateDate = contract.CreateDate,
                        LastUpdateDate = contract.LastUpdateDate
                    };
                    var insertedLandLease = landLeaseAgreementRepository.Insert(landLeaseAgreement);
                    foreach (var townshipId in model.TownShipIds)
                    {
                        var insertedJunction = townshipLandLeaseAgreementJunctionRepository.Insert(
                            new TownshipLandLeaseAgreementJunction
                            {
                                Id = 0,
                                LandLeaseAgreementId = insertedLandLease.Id,
                                TownshipId = townshipId
                            }
                            );
                    }
                }
                if (contractType != null && contractType.ContractTypeName == Constants.ContractTypeSUA)
                {
                    SurfaceUseAgreement surfaceUseAgreement = new SurfaceUseAgreement
                    {
                        Id = 0,
                        ContractId = contract.Id,
                        AltIdCategoryId = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdCategoryId : null,
                        AltIdInformation = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdInformation : null,
                        EffectiveDate = model.EffectiveDate,
                        Acreage = model.Acreage,
                        TerminationDate = model.TerminationDate,
                        TerminationReason = model.TerminationReason,
                        ReversionDate = model.ReversionDate,
                        CreatedBy = contract.CreatedBy,
                        UpdatedBy = contract.UpdatedBy,
                        CreateDate = contract.CreateDate,
                        LastUpdateDate = contract.LastUpdateDate
                    };
                    var insertedSurfaceUseAgreement = surfaceUseAgreementRepository.Insert(surfaceUseAgreement);
                    foreach (var townshipId in model.TownShipIds)
                    {
                        var insertedJunction = townshipSurfaceUseAgreementJunctionRepository.Insert(
                            new TownshipSurfaceUseAgreementJunction
                            {
                                Id = 0,
                                SurfaceUseAgreementId = insertedSurfaceUseAgreement.Id,
                                TownshipId = townshipId
                            }
                            );
                    }
                }

                if (contractType != null && contractType.ContractTypeName == Constants.ContractTypeProspecting)
                {
                    ProspectingAgreement prospectingAgreement = new ProspectingAgreement
                    {
                        Id = 0,
                        ContractId = contract.Id,
                        AltIdCategoryId = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdCategoryId : null,
                        AltIdInformation = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdInformation : null,
                        EffectiveDate = model.EffectiveDate,
                        Acreage = model.Acreage,
                        TerminationDate = model.TerminationDate,
                        TerminationReason = model.TerminationReason,
                        CreatedBy = contract.CreatedBy,
                        UpdatedBy = contract.UpdatedBy,
                        CreateDate = contract.CreateDate,
                        LastUpdateDate = contract.LastUpdateDate
                    };
                    var insertedProspectingAgreement = prospectingAgreementRepository.Insert(prospectingAgreement);

                    foreach (var townshipId in model.TownShipIds)
                    {
                        var insertedJunction = townshipProspectingAgreementJunctionRepository.Insert(
                            new TownshipProspectingAgreementJunction
                            {
                                Id = 0,
                                ProspectingAgreementId = insertedProspectingAgreement.Id,
                                TownshipId = townshipId
                            }
                            );
                    }
                }

                if (contractType != null && contractType.ContractTypeName == Constants.ContractTypeProduction)
                {
                    ProductionAgreement productionAgreement = new ProductionAgreement
                    {
                        Id = 0,
                        ContractId = contract.Id,
                        AltIdCategoryId = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdCategoryId : null,
                        AltIdInformation = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdInformation : null,
                        EffectiveDate = model.EffectiveDate,
                        Acreage = model.Acreage,
                        TerminationDate = model.TerminationDate,
                        TerminationReason = model.TerminationReason,
                        CreatedBy = contract.CreatedBy,
                        UpdatedBy = contract.UpdatedBy,
                        CreateDate = contract.CreateDate,
                        LastUpdateDate = contract.LastUpdateDate
                    };
                    var insertedProductionAgreement = productionAgreementRepository.Insert(productionAgreement);

                    foreach (var townshipId in model.TownShipIds)
                    {
                        var insertedJunction = townshipProductionAgreementJunctionRepository.Insert(
                            new TownshipProductionAgreementJunction
                            {
                                Id = 0,
                                ProductionAgreementId = insertedProductionAgreement.Id,
                                TownshipId = townshipId
                            }
                            );
                    }
                }

                if (contractType != null && contractType.ContractTypeName == Constants.ContractTypeSeismic)
                {
                    SeismicAgreement seismicAgreement = new SeismicAgreement
                    {
                        Id = 0,
                        ContractId = contract.Id,
                        Acreage = model.Acreage,
                        AltIdCategoryId = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdCategoryId : null,
                        AltIdInformation = (model.AltIdCategoryId != null && model.AltIdCategoryId > 0) ? model.AltIdInformation : null,
                        DataReceivedDate = model.DataReceivedDate,
                        EffectiveDate = model.EffectiveDate,
                        TerminationDate = model.TerminationDate,
                        TerminationReason = model.TerminationReason,
                        CreatedBy = contract.CreatedBy,
                        UpdatedBy = contract.UpdatedBy,
                        CreateDate = contract.CreateDate,
                        LastUpdateDate = contract.LastUpdateDate
                    };
                    var insertedSeismicAgreement = seismicAgreementRepository.Insert(seismicAgreement);
                    foreach (var townshipId in model.TownShipIds)
                    {
                        var insertedJunction = townshipSeismicAgreementJunctionRepository.Insert(
                            new TownshipSeismicAgreementJunction
                            {
                                Id = 0,
                                SeismicAgreementId = insertedSeismicAgreement.Id,
                                TownshipId = townshipId
                            }
                            );
                    }
                }

            }
            
        }

        public void AddAssociatedContracts(long contractId, string[] associatedContractNames)
        {
            foreach (var associatedContractName in associatedContractNames)
            {
                var retval = associatedContractRepository.Insert(new AssociatedContract { Id = 0, ContractId = contractId, AssociatedContractName = associatedContractName });
            }
        }

        public void AddAssociatedTracts(long contractId, string[] tractNums)
        {
            foreach (var tractNum in tractNums)
            {
                var retval = associatedTractRepository.Insert(new AssociatedTract { Id = 0, ContractId = contractId, TractNum= tractNum });
            }
        }

        public void AddCurrentEvents(long contractId, ContractEventDetail[] currentEvents, DateTime? lesseeEffectiveDate, decimal? acreage)
        {
            //// add a start contract event if there are no contracteventdetails for this contract
            bool addStartContract = false;

            long contractEventDetailReasonForChangeId = contractEventDetailReasonForChangeRepository
                                                        .GetAll()
                                                        .FirstOrDefault(x => x.Reason == Constants.ContractEventDetailReasonForChangeContractStart)
                                                        .Id;

            if (currentEvents != null && currentEvents.Length > 0 && !(genericContractEventDetailRepository.GetAll().Where(x => x.ContractId == contractId).Any())) {
                addStartContract = true;
            }

            List<ContractEventDetail> contractEventDetails = new List<ContractEventDetail>();
            foreach (var currentEvent in currentEvents)
            {
                contractEventDetails.Add(new ContractEventDetail
                {
                    Id = 0,
                    ContractId = contractId,
                    Acres = currentEvent.Acres,
                    ActiveInd = true,
                    InterestType = currentEvent.InterestType,
                    BottomVerticalExtentOfOwnership = currentEvent.BottomVerticalExtentOfOwnership,
                    TopVerticalExtentOfOwnership = currentEvent.TopVerticalExtentOfOwnership,
                    ExcludedFromVerticalExtentOfOwnership = currentEvent.ExcludedFromVerticalExtentOfOwnership,
                    LesseeId = currentEvent.LesseeId,
                    LesseeName = lesseeRepository.GetAll().FirstOrDefault(x => x.Id == currentEvent.LesseeId)?.LesseeName,
                    ShareOfLeasePercentage = currentEvent.ShareOfLeasePercentage,
                    Horizon = currentEvent.Horizon,
                    IndustryLeaseInd = currentEvent.IndustryLeaseInd,
                    RoyaltyPercent = currentEvent.RoyaltyPercent,
                    MinimumRoyalty = currentEvent.MinimumRoyalty,
                    MinimumRoyaltySalesPrice = currentEvent.MinimumRoyaltySalesPrice
                });

                if (addStartContract)
                {
                    contractEventDetails.Add(new ContractEventDetail
                    {
                        Id = 0,
                        ContractId = contractId,
                        Acres = currentEvent.Acres,
                        ActiveInd = false,
                        InterestType = currentEvent.InterestType,
                        BottomVerticalExtentOfOwnership = currentEvent.BottomVerticalExtentOfOwnership,
                        TopVerticalExtentOfOwnership = currentEvent.TopVerticalExtentOfOwnership,
                        ExcludedFromVerticalExtentOfOwnership = currentEvent.ExcludedFromVerticalExtentOfOwnership,
                        LesseeId = currentEvent.LesseeId,
                        LesseeName = lesseeRepository.GetAll().FirstOrDefault(x => x.Id == currentEvent.LesseeId)?.LesseeName,
                        ShareOfLeasePercentage = currentEvent.ShareOfLeasePercentage,
                        EffectiveDate = lesseeEffectiveDate,
                        CurrentTotalAcres = acreage,
                        PreviousTotalAcres = 0,
                        ContractEventDetailReasonForChangeId = contractEventDetailReasonForChangeId,
                        Horizon = currentEvent.Horizon,
                        IndustryLeaseInd = currentEvent.IndustryLeaseInd,
                        RoyaltyPercent = currentEvent.RoyaltyPercent,
                        MinimumRoyalty = currentEvent.MinimumRoyalty,
                        MinimumRoyaltySalesPrice = currentEvent.MinimumRoyaltySalesPrice
                    });
                }
            }
            if (contractEventDetails.Any())
            {
                contractEventDetailRepository.InsertAll(contractEventDetails);
            }
            
        }

        /// <summary> Replace a contracts districts</summary>
        public void AddDistrictContractJunctions(long id, long[] districtIds)
        {
            var districts = districtContractJunctionRepository.GetAll().Where(x => x.ContractId == id);
            foreach (var district in districts)
            {
                districtContractJunctionRepository.Delete(district.Id);
            }
            foreach (var item in districtIds)
            {
                var retVal = districtContractJunctionRepository.Insert(new DistrictContractJunction { Id = 0, ContractId = id, DistrictId = item });
            }
        }


        public void AddPaymentRequirement(long returnId, AddUpdateContractViewModel model)
        {
            var retVal = paymentRequirementRepository.Insert(new PaymentRequirement { Id = 0 });
        }

        public void AddRowContracts(long id, IEnumerable<string> rowContracts)
        {
            foreach (var item in rowContracts)
            {
                var retVal = rowContractRepository.Insert(new RowContract { Id = 0, ContractId = id, RowContractName = item });
            }
        }

        public void DeleteAgreements(long id)
        {
            var landLeaseAgreement = landLeaseAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == id);
            var surfaceUseAgreement = surfaceUseAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == id);
            var prospectingAgreement = prospectingAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == id);
            var productionAgreement = productionAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == id);
            var seismicAgreement = seismicAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == id);

            if (landLeaseAgreement != null)
            {
                var townshipJunctions = townshipLandLeaseAgreementJunctionRepository.GetAll().Where(x => x.LandLeaseAgreementId == landLeaseAgreement.Id);
                foreach (var townshipJunction in townshipJunctions)
                {
                    townshipLandLeaseAgreementJunctionRepository.Delete(townshipJunction.Id);
                }
                landLeaseAgreementRepository.Delete(landLeaseAgreement.Id);
            }
            
            if (surfaceUseAgreement != null)
            {
                var townshipJunctions = townshipSurfaceUseAgreementJunctionRepository.GetAll().Where(x => x.SurfaceUseAgreementId == surfaceUseAgreement.Id);
                foreach (var townshipJunction in townshipJunctions)
                {
                    townshipSurfaceUseAgreementJunctionRepository.Delete(townshipJunction.Id);
                }
                surfaceUseAgreementRepository.Delete(surfaceUseAgreement.Id);
            }

            if (prospectingAgreement != null)
            {
                var townshipJunctions = townshipProspectingAgreementJunctionRepository.GetAll().Where(x => x.ProspectingAgreementId == prospectingAgreement.Id);
                foreach (var townshipJunction in townshipJunctions)
                {
                    townshipProspectingAgreementJunctionRepository.Delete(townshipJunction.Id);
                }
                prospectingAgreementRepository.Delete(prospectingAgreement.Id);
            }

            if (productionAgreement != null)
            {
                var townshipJunctions = townshipProductionAgreementJunctionRepository.GetAll().Where(x => x.ProductionAgreementId == productionAgreement.Id);
                foreach (var townshipJunction in townshipJunctions)
                {
                    townshipProductionAgreementJunctionRepository.Delete(townshipJunction.Id);
                }
                productionAgreementRepository.Delete(productionAgreement.Id);
            }

            if (seismicAgreement != null)
            {
                seismicAgreementRepository.Delete(seismicAgreement.Id);
            }

        }

        public void DeleteAssociatedContracts(long id)
        {
            var associatedContracts = associatedContractRepository.GetAll().Where(x => x.ContractId == id);
            foreach (var associatedContract in associatedContracts)
            {
                associatedContractRepository.Delete(associatedContract.Id);
            }
        }

        public void DeleteAssociatedTracts(long id)
        {
            var associatedTracts = associatedTractRepository.GetAll().Where(x => x.ContractId == id);
            foreach (var associatedTract in associatedTracts)
            {
                associatedTractRepository.Delete(associatedTract.Id);
            }
        }

        public void DeleteCurrentEvents(
            long id, IEnumerable<ContractEventDetail> contractEventDetails, long reasonForChangeId,
            decimal? initialAcreage, decimal? currentAcreage, DateTime? LesseeEffectiveDate, string reasonForChangeDescription)
        {
            var currentEvents = genericContractEventDetailRepository.GetAll().Where(x => x.ContractId == id && x.ActiveInd);
            if (reasonForChangeId > 0)
            {
                List<ContractEventDetail> currentEventDetailsToInsert = new List<ContractEventDetail>();
              

                foreach (var currentEvent in currentEvents)
                {

                    currentEventDetailsToInsert.Add(new ContractEventDetail
                    {
                        Id = 0,
                        ContractId = id,
                        ActiveInd = false,
                        EffectiveDate = LesseeEffectiveDate,
                        ContractEventDetailReasonForChangeId = reasonForChangeId,
                        Acres = currentEvent.Acres,
                        PreviousTotalAcres = initialAcreage,
                        CurrentTotalAcres = currentAcreage,
                        LesseeId = currentEvent.LesseeId,
                        LesseeName = currentEvent.LesseeName,
                        ShareOfLeasePercentage = currentEvent.ShareOfLeasePercentage,
                        InterestType = currentEvent.InterestType,
                        TopVerticalExtentOfOwnership = currentEvent.TopVerticalExtentOfOwnership,
                        BottomVerticalExtentOfOwnership = currentEvent.BottomVerticalExtentOfOwnership,
                        ExcludedFromVerticalExtentOfOwnership = currentEvent.ExcludedFromVerticalExtentOfOwnership,
                        Horizon = currentEvent.Horizon,
                        Description = reasonForChangeDescription,
                        IndustryLeaseInd = currentEvent.IndustryLeaseInd,
                        RoyaltyPercent = currentEvent.RoyaltyPercent,
                        MinimumRoyalty = currentEvent.MinimumRoyalty,
                        MinimumRoyaltySalesPrice = currentEvent.MinimumRoyaltySalesPrice
                    });
                }
                if (currentEventDetailsToInsert.Any())
                {
                    contractEventDetailRepository.InsertAll(currentEventDetailsToInsert);
                }

            }

            foreach (var currentEvent in currentEvents)
            {
                genericContractEventDetailRepository.Delete(currentEvent.Id);
            }

        }

        public void DeleteRowContracts(long id)
        {
            var rowContracts = rowContractRepository.GetAll().Where(x => x.ContractId == id);
            foreach (var rowContract in rowContracts)
            {
                rowContractRepository.Delete(rowContract.Id);
            }
        }

        /// <summary> Find a contracts tract by its tractnum. If if does not exist create a new one</summary>
        public Tract getTractByTractNum(string tractNum, long? contractTypeId)
        {
            Tract tract = new Tract();
            if (tractNum != null && !string.IsNullOrEmpty(tractNum))
            {
                tract = genericTractRepository.GetAll().FirstOrDefault(x => x.TractNum.ToLower(ci) == tractNum.ToLower(ci));
                if (tract == null)
                {
                    tract = new Tract
                    {
                        Id = 0,
                        TractNum = tractNum
                    };
                    tract = genericTractRepository.Insert(tract);
                }
                else
                {
                    if (contractTypeId != null)
                    {
                        var contractType = contractTypeRepository.GetById(contractTypeId.Value);
                        if (contractType.ContractTypeName != Constants.ContractTypeSUA && tract.Administrative == true)
                        {
                            tract = tractRepository.UpdateAdminTract(tract.Id);
                        }

                    }
                }
            }
            return tract;
        }

        public void UpdateContractRentalPaymentMonths(long returnId, IEnumerable<long> contractRentalPaymentMonthIds)
        {
    
            foreach (var item in contractRentalPaymentMonthJunctionRepository.GetAll().Where(x => x.ContractId == returnId).ToList())
            {
                contractRentalPaymentMonthJunctionRepository.Delete(item.Id);
            }
            if (contractRentalPaymentMonthIds != null)
            {
                foreach (var item in contractRentalPaymentMonthIds)
                {
                    contractRentalPaymentMonthJunctionRepository.Insert(new ContractRentalPaymentMonthJunction
                    {
                        Id = 0,
                        MonthId = item,
                        ContractId = returnId
                    });
                }
            }
        }

        public void UpdateStorage(
            long returnId,
            long? contractSubTypeId,
            Storage storage,
            IEnumerable<long> storageWellPaymentMonthIds,
            IEnumerable<long> storageBaseRentalPaymentMonthIds,
            IEnumerable<long> storageRentalPaymentMonthIds,
            long userId)
        {
            ContractSubType contractSubType = contractSubTypeRepository.GetAll().First(x => x.ContractSubTypeName == Constants.ContractSubTypeGasStorageField);
            if (contractSubType.Id == contractSubTypeId && storage != null)
            {
                if (storage.Id == 0)
                {
                    List<StorageWellPaymentMonthJunction> storageWellPaymentMonthJunctions = new List<StorageWellPaymentMonthJunction>();
                    foreach (long monthId in storageWellPaymentMonthIds)
                    {
                        storageWellPaymentMonthJunctions.Add(new StorageWellPaymentMonthJunction { MonthId = monthId });
                    }
                    List<StorageBaseRentalPaymentMonthJunction> storageBaseRentalPaymentMonthJunctions = new List<StorageBaseRentalPaymentMonthJunction>();
                    foreach (long monthId in storageBaseRentalPaymentMonthIds)
                    {
                        storageBaseRentalPaymentMonthJunctions.Add(new StorageBaseRentalPaymentMonthJunction { MonthId = monthId });
                    }
                    List<StorageRentalPaymentMonthJunction> storageRentalPaymentMonthJunctions = new List<StorageRentalPaymentMonthJunction>();
                    foreach (long monthId in storageRentalPaymentMonthIds)
                    {
                        storageRentalPaymentMonthJunctions.Add(new StorageRentalPaymentMonthJunction { MonthId = monthId });
                    }

                    genericStorageRepository.Insert(new Storage
                    {
                        Id = 0,
                        ContractId = returnId,
                        LeaseNum = storage.LeaseNum,
                        StorageWellPaymentMonthJunctions = storageWellPaymentMonthJunctions,
                        StorageBaseRentalPaymentMonthJunctions = storageBaseRentalPaymentMonthJunctions,
                        StorageRentalPaymentMonthJunctions = storageRentalPaymentMonthJunctions,
                        StorageWellPayment = storage.StorageWellPayment,
                        StorageBaseRentalPayment = storage.StorageBaseRentalPayment,
                        StorageRentalPayment = storage.StorageRentalPayment,
                        SubjectToInflationInd = storage.SubjectToInflationInd,
                        CreateDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                    });
                }
                else
                {
                    storageRepository.Update(storage, storageWellPaymentMonthIds, storageBaseRentalPaymentMonthIds, storageRentalPaymentMonthIds, userId);
                }
            }
        }


        public void UpdateWellOwnership(long id, long reasonForChangeId, DateTime? lesseeEffectiveDate)
        {
            long contractEventDetailReasonForChangeCorrectionId = contractEventDetailReasonForChangeRepository
                                                      .GetAll()
                                                      .FirstOrDefault(x => x.Reason == Constants.ContractEventDetailReasonForChangeCorrection)
                                                      .Id;
            List<Well> allWells = wellRepository.GetAll().Where(x => x.AutoUpdatedAllowedInd == true).ToList();
            long[] wellIds = genericWellTractInfoRepository.GetAll().Where(x => x.ContractId == id && x.ActiveInd == true).Select(x => x.WellId).Distinct().ToArray();
            List<Well> wells = new List<Well>();
            foreach (long wellid in wellIds)
            {
                var well = allWells.FirstOrDefault(x => x.Id == wellid);
                if (well != null)
                {
                    wells.Add(well);
                }
            }
            Contract contract = contractRepository.GetById(id);
            List<ContractEventDetail> contractEventDetais = genericContractEventDetailRepository.GetAll().Where(x => x.ActiveInd == true && x.ContractId == id).ToList();
            List<Pad> allPads = padRepository.GetAll().ToList();
            List<WellTractInformation> wellTractInformations = new List<WellTractInformation>();
            foreach (var well in wells)
            {
                if (well.AutoUpdatedAllowedInd == true )
                {
                    wellTractInfoRepository.SetActiveRecordsToHistoric(well.Id, reasonForChangeId, lesseeEffectiveDate.Value);
                }
                
                foreach (var item in contractEventDetais)
                {
                    // try and find the pad
                    long? padId = null;

                    WellTractInformation correspondingHistoricalRecord = genericWellTractInfoRepository.GetAll()
                         .Where(x => x.ActiveInd == false && x.WellId == well.Id && x.TractId == contract.TractId && x.PadId != null)
                         .OrderByDescending(x => x.ChangeDate)
                         .FirstOrDefault();
                    if (correspondingHistoricalRecord != null)
                    {
                        padId = correspondingHistoricalRecord.PadId;
                    } else
                    {
                        List<Pad> pads = allPads.Where(x => x.TractId == contract.TractId).ToList();
                        if (pads.Count == 1)
                        {
                            padId = pads.FirstOrDefault().Id;
                        }
                    }



                    wellTractInformations.Add(new WellTractInformation {
                       Id = 0,
                       ActiveInd = true,
                       WellId = well.Id,
                       TractId = contract.TractId,
                       PadId = padId,
                       LesseeId = item.LesseeId,
                       LesseeName = item.LesseeName,
                       ContractId = contract.Id,
                       PercentOwnership = item.ShareOfLeasePercentage,
                       RoyaltyPercent = item.RoyaltyPercent
                    });
                }
            }
            if (wellTractInformations.Any())
            {
                wellTractInfoRepository.InsertAll(wellTractInformations);
            }
            

        }
    }
}
