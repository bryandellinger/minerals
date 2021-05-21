using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.BusinessLogic
{
    public class StorageRentalBusinessLogic : IStorageRentalBusinessLogic
    {
        private IGenericRepository<StorageRental> genericStorageRentalRepository;
        private IStorageRentalRepository storageRentalRepository;

        public StorageRentalBusinessLogic(
            IGenericRepository<StorageRental> genericStorageRentalRepo,
             IStorageRentalRepository storageRentalRepo
            )
        {
            genericStorageRentalRepository = genericStorageRentalRepo;
            storageRentalRepository = storageRentalRepo;
        }

        public object Save(StorageRental model, long id)
        {
            if (model.Id > 0)
            {
                storageRentalRepository.Update(model, id);
                return new StorageRental { Id = model.Id };
            }
            else
            {
                StorageRental newStorageRental= new StorageRental
                {
                    Id = 0,
                    StorageId = model.StorageId,
                    CheckId = model.CheckId,
                    PeriodTypeId = model.PeriodTypeId,
                    StorageRentalPaymentTypeId = model.StorageRentalPaymentTypeId,
                    RentPay = model.RentPay,
                    StorageRentalEntryDate = model.StorageRentalEntryDate,
                    PaymentPeriodYear = model.PaymentPeriodYear,
                    Well = model.Well,
                    StorageRentalNotes = model.StorageRentalNotes,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CreatedBy = id,
                    UpdatedBy = id,
                };

                genericStorageRentalRepository.Insert(newStorageRental);
                return new StorageRental{ Id = newStorageRental.Id };
            }
        }
    }
}
