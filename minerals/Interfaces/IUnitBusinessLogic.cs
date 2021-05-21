using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;

namespace Minerals.Interfaces
{
    public interface IUnitBusinessLogic
    {
        object Save(UnitViewModel model, long userId);
    }
}
