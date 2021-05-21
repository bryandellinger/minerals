using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface ISurfaceUseAgreementRepository
    {
        Task<SurfaceUseAgreement> GetSurfaceUseAgreementByContractAsync(long Id);
    }
}
