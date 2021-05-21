using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IUploadPaymentBusinessLogic
    {
        object Save(UploadPayment model, long id);
        object CreateCheck(CreateCheckViewModel model, long id);
        object CreateCSVPayments(long fileId, long uploadTemplateId);
    }
}
