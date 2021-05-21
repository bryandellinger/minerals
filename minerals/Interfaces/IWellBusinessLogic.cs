using Microsoft.AspNetCore.Http;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IWellBusinessLogic
    {
        WellViewModel Save(WellViewModel model, long userId);
    }
}
