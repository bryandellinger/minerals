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
    public class LesseeContactBusinessLogic : ILesseeContactBusinessLogic
    {
        private readonly IGenericRepository<LesseeContact> lesseeContactRepository;


        public LesseeContactBusinessLogic(IGenericRepository<LesseeContact> lesseeContactRepo) => lesseeContactRepository = lesseeContactRepo;
        public long AddLesseeContact(LesseeContact model, long userId)
        {
            LesseeContact existingContact = lesseeContactRepository.GetById(model.Id);
            if (existingContact != null)
            {
                lesseeContactRepository.Delete(model.Id);
            }
            
        

            LesseeContact newLesseeContact = new LesseeContact
            {
                Id = 0,
                LesseeId = model.LesseeId,
                CreateDate = existingContact != null ? existingContact.CreateDate : DateTime.Now,
                LastUpdateDate = DateTime.Now,
                CreatedBy = existingContact != null ? existingContact.CreatedBy : userId,
                UpdatedBy = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Title = model.Title,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                State = model.State,
                Zip = model.Zip,
                Phone1 = model.Phone1,
                Phone2 = model.Phone2,
                Fax = model.Fax,
                Email = model.Email,
                Notes = model.Notes
            };

            LesseeContact insertedContact = lesseeContactRepository.Insert(newLesseeContact);

            return insertedContact.Id;
        }

    }
}
