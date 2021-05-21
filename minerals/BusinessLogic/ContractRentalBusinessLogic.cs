using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.BusinessLogic
{
    public class ContractRentalBusinessLogic : IContractRentalBusinessLogic
    {
        private IGenericRepository<ContractRental> genericContractRentalRepository;
        private IContractRentalRepository contractRentalRepository;

        public ContractRentalBusinessLogic(
            IGenericRepository<ContractRental> genericContractRentalRepo,
            IContractRentalRepository contractRentalRepo
            )
        {
            genericContractRentalRepository = genericContractRentalRepo;
            contractRentalRepository = contractRentalRepo;
        }
        public object Save(ContractRental model, long id)
        {
            if (model.Id > 0)
            {
                contractRentalRepository.Update(model, id);
                return new ContractRental { Id = model.Id };
            }
            else
            {
                ContractRental newContractRental = new ContractRental
                {
                    Id = 0,
                    ContractId = model.ContractId,
                    CheckId = model.CheckId,
                    PeriodTypeId = model.PeriodTypeId,
                    ContractRentPay = model.ContractRentPay,
                    ContractRentalEntryDate = model.ContractRentalEntryDate,
                    ContractPaymentPeriodYear = model.ContractPaymentPeriodYear,
                    HeldByProduction = model.HeldByProduction,
                    ContractRentalNotes = model.ContractRentalNotes,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CreatedBy = id,
                    UpdatedBy = id,
                };

                genericContractRentalRepository.Insert(newContractRental);
                return new ContractRental { Id = newContractRental.Id };
            }
        }
    }
}
