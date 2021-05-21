using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;

namespace Minerals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeUser(Roles = "read")]
    public class ContractMgrApiController : ControllerBase
    {
        private readonly IGenericRepository<Contract> genericrepository;
        private readonly ICurrentUserRepository currentUserRepository;
        private readonly IContractBusinessLogic contractBusinessLogic;
        private readonly IContractRepository contractrepository;
     

        public ContractMgrApiController(
            IGenericRepository<Contract> genericrepo,
            ICurrentUserRepository currentUserRepo,
            IContractBusinessLogic contBusLog,
            IContractRepository contractrepo)
        {
            genericrepository = genericrepo;
            contractrepository = contractrepo;
            currentUserRepository = currentUserRepo;
            contractBusinessLogic = contBusLog;
        }

        /// <summary> Returns all Contracts as JSON</summary>
        [HttpGet()]
        public async Task<IActionResult> Get() => Ok((await genericrepository.GetAllAsync().ConfigureAwait(false)).OrderBy(x => x.ContractNum));

        /// <summary> Returns Contract given a contract id</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => Ok(await genericrepository.GetByIdAsync(id).ConfigureAwait(false));

        [HttpGet("contractsByWell/{id}")]
        public async Task<IActionResult> GetContractsByWell(long id) =>
            Ok((await contractrepository.GetContractsByWellAsync(id).ConfigureAwait(false)).OrderBy(x => x.ContractNum));
        

        [HttpGet("contractsByTract/{id}")]
        public async Task<IActionResult> GetByTract(long id) => 
            Ok((await genericrepository.GetAllAsync().ConfigureAwait(false)).Where(x => x.TractId == id).OrderBy(x => x.ContractNum));

        [HttpGet("GetByStorageRental/{id}")]
        public async Task<IActionResult> GetByStorageRental(long id) =>
         Ok(await contractrepository.GetByStorageRentalAsync(id).ConfigureAwait(false));

        [HttpGet("GetByContractRental/{id}")]
        public async Task<IActionResult> GetByContractRental(long id) =>
        Ok(await contractrepository.GetByContractRentalAsync(id).ConfigureAwait(false));

        [HttpPost("GetStorageRentalInformation")]
        public async Task<IActionResult> GetStorageRentalInformation([FromBody] StorageRentalInformationViewModel model ) =>
        Ok(await contractrepository.GetStorageRentalInformationAsync(
            model.Id, model.LeasedTractAcreage, model.EffectiveDate, model.FifthYearOnwardDelayRental, model.SecondThroughFourthYearDelayRental
            ).ConfigureAwait(false));

        [HttpGet("GetForStorageRental")]
        public async Task<IActionResult> GetForStorageRental() =>
        Ok(await contractrepository.GetForStorageRentalAsync().ConfigureAwait(false));
  
        [HttpGet("GetForContractRental/{id}")]
        public async Task<IActionResult> GetForContractRental(long id) =>
        Ok(await contractrepository.GetForContractRentalAsync(id).ConfigureAwait(false));

        [HttpGet("GetByLessee/{id}")]
        public async Task<IActionResult> GetByLessee(long id) =>
            Ok(await contractrepository.GetByLesseeAsync(id).ConfigureAwait(false));

        /// <summary> Used for a new or edited contract. if model.id is 0 it is a new contract otherwise an edit. contracts are replaced not updated.
        /// 3 tables are affected tract, contract, and contractDistrictJunction. </summary>
        [HttpPost]
        public IActionResult Post([FromBody]  AddUpdateContractViewModel model)
        {
            long returnId;
            List<AdditionalBonus> additionalBonuses = new List<AdditionalBonus>();
            foreach (var additionalBonusAmount in model.AdditionalBonuses)
            {
                additionalBonuses.Add(new AdditionalBonus { AdditionalBonusAmount = additionalBonusAmount });
            }
    
            Contract contract = new Contract
            {
                Id = 0,
                ContractNum = model.ContractNum,
                CreatedBy = model.Id > 0 ? genericrepository.GetById(model.Id)?.CreatedBy : currentUserRepository.Get(HttpContext, User.Identity.Name).Id,
                CreateDate = model.Id > 0 ? genericrepository.GetById(model.Id)?.CreateDate : DateTime.Now,
                LastUpdateDate = DateTime.Now,
                UpdatedBy = currentUserRepository.Get(HttpContext, User.Identity.Name).Id,
                TractId = model.TractNum == null ? null : contractBusinessLogic.getTractByTractNum(model.TractNum, model.ContractTypeId)?.Id,
                ContractTypeId = model.ContractTypeId,
                ContractSubTypeId = model.ContractSubTypeId,
                Sequence = model.Sequence,
                ContractNumOverride = model.ContractNumOverride,
                ContractRentalPayment = model.ContractRentalPayment,
                ContractRentalPaymentOverride = model.ContractRentalPaymentOverride,
                AdditionalContractInformationId = model.AdditionalContractInformationId,
                PaymentRequirement = new PaymentRequirement
                {
                    Id = 0,
                    CheckSubmissionPeriod = model.CheckSubmissionPeriod,
                    FirstYearRentalBonusAmount = model.FirstYearRentalBonusAmount,
                    TotalBonusAmount = model.TotalBonusAmount,
                    LeaseExtensionBonus = model.LeaseExtensionBonus,
                    RentalShutInDueDate = model.RentalShutInDueDate,
                    SecondThroughFourthYearDelayRental = model.SecondThroughFourthYearDelayRental,
                    SecondThroughFourthYearShutInRate = model.SecondThroughFourthYearShutInRate,
                    FifthYearOnwardDelayRental = model.FifthYearOnwardDelayRental,
                    FifthYearOnwardShutInRate = model.FifthYearOnwardShutInRate,
                    ShutInPaymentInterval = model.ShutInPaymentInterval,
                    SecondThroughFourthYearShutInRateInterval = model.SecondThroughFourthYearShutInRateInterval,
                    StorageFee = model.StorageFee,
                    AllowableVariancePerAuditFieldPercent = model.AllowableVariancePerAuditFieldPercent,
                    ProducerPriceIndex = model.ProducerPriceIndex,
                    FiveYearInflationIntervalPeriodInd = model.FiveYearInflationIntervalPeriodInd,
                    TestOfWellEconomy = model.TestOfWellEconomy,
                    PerformanceSurety = model.PerformanceSurety,
                    PluggingSuretyDetails = model.PluggingSuretyDetails,
                    AgreementFee = model.AgreementFee,
                    AdditionalBonuses = additionalBonuses
                }
            };

            if (contract.TractId < 1)
            {
                contract.TractId = null;
            }

            if (model.Id > 0)
            {
                contractBusinessLogic.DeleteAgreements(model.Id);
                contractBusinessLogic.DeleteRowContracts(model.Id);
                contractBusinessLogic.DeleteAssociatedContracts(model.Id);
                contractBusinessLogic.DeleteAssociatedTracts(model.Id);
                contractBusinessLogic.DeleteCurrentEvents(
                    model.Id, model.ContractEventDetails, model.ReasonForChangeId, model.InitialAcreage, model.Acreage, model.LesseeEffectiveDate, model.ReasonForChangeDescription
                    );
           
               
            }
            returnId = model.Id > 0 ? contractrepository.Update(model.Id,contract).Id : genericrepository.Insert(contract).Id;

            contractBusinessLogic.AddDistrictContractJunctions(returnId, model.DistrictIds.ToArray());
            contractBusinessLogic.AddAgreement(returnId, model);
            contractBusinessLogic.AddRowContracts(returnId, model.RowContracts);
            contractBusinessLogic.AddAssociatedContracts(returnId, model.AssociatedContracts.ToArray());
            contractBusinessLogic.AddAssociatedTracts(returnId, model.AssociatedTracts.ToArray());
            contractBusinessLogic.AddCurrentEvents(returnId, model.ContractEventDetails.ToArray(), model.LesseeEffectiveDate, model.Acreage);
            if (model.Id > 0 && model.updateWellInfoInd == true)
            {
                contractBusinessLogic.UpdateWellOwnership(model.Id, model.ReasonForChangeId, model.LesseeEffectiveDate);
            }
            contractBusinessLogic.UpdateStorage(
                returnId,
                model.ContractSubTypeId,
                model.Storage,
                model.StorageWellPaymentMonthIds,
                model.StorageBaseRentalPaymentMonthIds,
                model.StorageRentalPaymentMonthIds,
                currentUserRepository.Get(HttpContext, User.Identity.Name).Id);
            contractBusinessLogic.UpdateContractRentalPaymentMonths(returnId, model.ContractRentalPaymentMonthIds);
            return Ok(new Contract { Id = returnId});
        }
    }
}