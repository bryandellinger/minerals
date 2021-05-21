using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IStorageRentalBusinessLogic
    {
        object Save(StorageRental model, long id);
    }
}
