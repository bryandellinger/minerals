using Minerals.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.BusinessLogic
{
    public class SuretyBusinessLogic  : ISuretyBusinessLogic
    {
        private IGenericRepository<Surety> genericRepository;
        private ISuretyRepository repository;

        public SuretyBusinessLogic(IGenericRepository<Surety> genericRepo, ISuretyRepository repo)
        {
            genericRepository = genericRepo;
            repository = repo;
        }

        public object Save(Surety model, long id)
        {
            if (model.Id > 0)
            {
                repository.DeleteSuretyRiders(model.Id);
                repository.DeleteSuretyWells(model.Id);
                repository.Update(model, id);
                if (model.SuretyRiders.Any())
                {
                    repository.AddSuretyRiders(model.SuretyRiders, model.Id);
                }
                if (model.SuretyWells.Any())
                {
                    repository.AddSuretyWells(model.SuretyWells, model.Id);
                }
                return new Surety { Id = model.Id };
            }
            else
            {
                List<SuretyRider> suretyRiders = new List<SuretyRider>();

                foreach (var item in model.SuretyRiders)
                {
                    suretyRiders.Add(new SuretyRider
                    {
                        RiderReasonId = item.RiderReasonId,
                        EffectiveDate = item.EffectiveDate
                    });
                }

                List<SuretyWell> suretyWells = new List<SuretyWell>();

                foreach (var item in model.SuretyWells)
                {
                    suretyWells.Add(new SuretyWell
                    {
                        WellId= item.WellId,
                        SuretyWellValue = item.SuretyWellValue
                    });
                }

                Surety newSurety= new Surety
                {
                    Id = 0,
                    SuretyTypeId = model.SuretyTypeId,
                    BondCategoryId = model.BondCategoryId,
                    IssueDate = model.IssueDate,
                    LesseeId= model.LesseeId,
                    ContractId=model.ContractId,
                    Insurer= model.Insurer,
                    SuretyNum = model.SuretyNum,
                    SuretyNotes = model.SuretyNotes,
                    InitialSuretyValue =model.InitialSuretyValue,
                    CurrentSuretyValue=model.CurrentSuretyValue,
                    ReleasedSuretyValue = model.ReleasedSuretyValue,
                    ClaimedInd = model.ClaimedInd,
                    SuretyStatus = model.SuretyStatus,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CreatedBy = id,
                    UpdatedBy = id,
                    SuretyRiders = suretyRiders,
                    SuretyWells = suretyWells
                };

                genericRepository.Insert(newSurety);
                return new OtherRental { Id = newSurety.Id };
            }
        }
    }
}
