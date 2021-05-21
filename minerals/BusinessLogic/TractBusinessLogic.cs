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
    public class TractBusinessLogic : ITractBusinessLogic
    {
        private readonly IGenericRepository<Tract> genericTractRepository;
        private readonly ITractRepository tractRepository;

        public TractBusinessLogic(IGenericRepository<Tract> genericTractRepo, ITractRepository tractRepo)
        {
            genericTractRepository = genericTractRepo;
            tractRepository = tractRepo;

        }

        public Tract Save(Tract model)
        {
            if (model.Id < 1)
            {
                return genericTractRepository.Insert(model);
            } else
            {               
                return tractRepository.Update(model); 
            }
        }
    }
}
