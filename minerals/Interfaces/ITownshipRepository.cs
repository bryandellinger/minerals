using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface ITownshipRepository
    {
        List<Township> GetAllTownshipsByContract(long Id);
    }
}
