using Minerals.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.BusinessLogic
{
    public class OtherPaymentBusinessLogic : IOtherPaymentBusinessLogic
    {
        private IGenericRepository<OtherRental> genericRepository;
        private IOtherPaymentRepository repository;

        public OtherPaymentBusinessLogic(
             IGenericRepository<OtherRental> genericRepo,
             IOtherPaymentRepository repo
            )
        {
            genericRepository = genericRepo;
            repository = repo;
        }
        public object Save(OtherRental model, long id)
        {
            if (model.Id > 0)
            {
                repository.Update(model, id);
                return new OtherRental { Id = model.Id };
            }
            else
            {
                OtherRental newOtherRental = new OtherRental
                {
                    Id = 0,
                    CheckId = model.CheckId,
                    OtherPaymentType= model.OtherPaymentType,
                    OtherRentPay = model.OtherRentPay,
                    OtherRentalEntryDate = model.OtherRentalEntryDate,
                    OtherRentalNotes = model.OtherRentalNotes,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CreatedBy = id,
                    UpdatedBy = id,
                };

                genericRepository.Insert(newOtherRental);
                return new OtherRental { Id = newOtherRental.Id };
            }
        }
    }
}
