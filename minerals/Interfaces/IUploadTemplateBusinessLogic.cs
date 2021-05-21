using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IUploadTemplateBusinessLogic
    {
        object Save(UploadTemplate model, long id);
    }
}
