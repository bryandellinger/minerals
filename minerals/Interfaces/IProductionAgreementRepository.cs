using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IProductionAgreementRepository
    {
        Task<ProductionAgreement> GetProductionAgreementByContractAsync(long Id);
    }
}
