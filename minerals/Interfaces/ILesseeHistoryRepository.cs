using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Minerals.Interfaces
{
    public interface ILesseeHistoryRepository
    {
        public Task<IEnumerable<LesseeHistory>> GetLesseeHistoryByLesseeAsync(long Id);
    }
}
