using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IPaymentRequirementRepository
    {
        Task<PaymentRequirement> GetPaymentRequirementByContractAsync(long Id);
        Task<object> GetPaymentRequirementsByContractAsync(long[] v);
    }
}
