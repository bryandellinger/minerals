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
    public class LesseeBusinessLogic : ILesseeBusinessLogic
    {
        private readonly IGenericRepository<Lessee> lesseeRepository;
        private readonly IGenericRepository<ContractEventDetail> contractEventDetailRepository;
        private readonly IGenericRepository<ContractEventDetailReasonForChange> contractEventDetailReasonForChangeRepository;
        private readonly IGenericRepository<LandLeaseAgreement> landLeaseAgreementRepository;
        private readonly IGenericRepository<ProductionAgreement> productionAgreementRepository;
        private readonly IGenericRepository<ProspectingAgreement> prospectingAgreementRepository;
        private readonly IGenericRepository<SeismicAgreement> seismicAgreementRepository;
        private readonly IGenericRepository<SurfaceUseAgreement> surfaceUseAgreementRepository;
        private readonly IGenericRepository<WellTractInformation> genericWellTractInfoRepository;
        private readonly IWellTractInformationRepository wellTractInfoRepository;

        public LesseeBusinessLogic(
            IGenericRepository<Lessee> lesseeRepo,
            IGenericRepository<ContractEventDetail> contractEventDetailRepo,
            IGenericRepository<ContractEventDetailReasonForChange> contractEventDetailReasonForChangeRepo,
            IGenericRepository<LandLeaseAgreement> landLeaseAgreementRepo,
            IGenericRepository<ProductionAgreement> productionAgreementRepo,
            IGenericRepository<ProspectingAgreement> prospectingAgreementRepo,
            IGenericRepository<SeismicAgreement> seismicAgreementRepo,
            IGenericRepository<SurfaceUseAgreement> surfaceUseAgreementRepo,
            IGenericRepository<WellTractInformation> genericWellTractInfoRepo,
            IWellTractInformationRepository wellTractInfoRepo
            )
        {
            lesseeRepository = lesseeRepo;
            contractEventDetailRepository = contractEventDetailRepo;
            contractEventDetailReasonForChangeRepository = contractEventDetailReasonForChangeRepo;
            landLeaseAgreementRepository = landLeaseAgreementRepo;
            productionAgreementRepository = productionAgreementRepo;
            prospectingAgreementRepository = prospectingAgreementRepo;
            seismicAgreementRepository = seismicAgreementRepo;
            surfaceUseAgreementRepository = surfaceUseAgreementRepo;
            genericWellTractInfoRepository = genericWellTractInfoRepo;
            wellTractInfoRepository = wellTractInfoRepo;
        }

        public void CreateHistoricalWellOwnershipRecords(long id, bool addHistoryInd)
        {
            Lessee lessee = lesseeRepository.GetById(id);

            long reasonForChangeId = contractEventDetailReasonForChangeRepository
              .GetAll()
              .Where(x => x.Reason == Constants.ContractEventDetailReasonForChangeNameChange)
              .First().Id;
            if (addHistoryInd == true)
            {
                List<WellTractInformation> wellTractInformations = genericWellTractInfoRepository.GetAll().ToList();
                foreach (var item in wellTractInformations.Where(x => x.LesseeId == id && x.ActiveInd))
                {
                    var wellTractInformation = genericWellTractInfoRepository.Insert(new WellTractInformation
                    {
                        Id = 0,
                        ChangeDate = DateTime.Now,
                        ContractEventDetailReasonForChangeId = reasonForChangeId,
                        LesseeId = item.LesseeId,
                        LesseeName = item.LesseeName,
                        ContractId = item.ContractId,
                        ActiveInd = false,
                        PadId = item.PadId,
                        PercentOwnership = item.PercentOwnership,
                        RoyaltyPercent = item.RoyaltyPercent,
                        TractId = item.TractId,
                        WellId = item.WellId
                    });
                }
            }
            wellTractInfoRepository.UpdateOwnershipLesseeName(id, lessee.LesseeName);

        }

        public void CreateHistoricalContractEventDetailRecords(long id)
        {
            Lessee lessee = lesseeRepository.GetById(id);

            long reasonForChangeId = contractEventDetailReasonForChangeRepository
                .GetAll()
                .Where(x => x.Reason == Constants.ContractEventDetailReasonForChangeNameChange)
                .First().Id;

            List<ContractEventDetail> contractEventDetails = contractEventDetailRepository.GetAll().ToList();

            foreach (var item in contractEventDetails.Where(x => x.LesseeId == id && x.ActiveInd))

            {

            
                var totalAcres = GetTotalAcres(item.ContractId);

                var newContractEventDetail = contractEventDetailRepository.Insert(new ContractEventDetail
                {
                    Id = 0,
                    ContractId = item.ContractId,
                    ActiveInd = false,
                    EffectiveDate = null,
                    ContractEventDetailReasonForChangeId = reasonForChangeId,
                    Acres = item.Acres,
                    CurrentTotalAcres = totalAcres,
                    PreviousTotalAcres = totalAcres,
                    LesseeId = item.LesseeId,
                    LesseeName = item.LesseeName,
                    ShareOfLeasePercentage = item.ShareOfLeasePercentage,
                    InterestType = item.InterestType,
                    TopVerticalExtentOfOwnership = item.TopVerticalExtentOfOwnership,
                    BottomVerticalExtentOfOwnership = item.BottomVerticalExtentOfOwnership,
                    ExcludedFromVerticalExtentOfOwnership = item.ExcludedFromVerticalExtentOfOwnership,
                    Horizon = item.Horizon,
                    IndustryLeaseInd = item.IndustryLeaseInd
                });
            }
        }


        private decimal? GetTotalAcres(long contractId)
        {
            LandLeaseAgreement landLeaseAgreement = landLeaseAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == contractId);
            if (landLeaseAgreement != null)
            {
                return landLeaseAgreement.Acreage;
            }
            ProductionAgreement productionAgreement = productionAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == contractId);
            if (productionAgreement != null)
            {
                return productionAgreement.Acreage;
            }
            ProspectingAgreement prospectingAgreement = prospectingAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == contractId);
            if (prospectingAgreement != null)
            {
                return prospectingAgreement.Acreage;
            }
            SeismicAgreement seismicAgreement = seismicAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == contractId);
            if (seismicAgreement != null)
            {
                return seismicAgreement.Acreage;
            }
            SurfaceUseAgreement surfaceUseAgreement = surfaceUseAgreementRepository.GetAll().FirstOrDefault(x => x.ContractId == contractId);
            if (surfaceUseAgreement != null)
            {
                return surfaceUseAgreement.Acreage;
            }
            return null;

        }
    }
}
