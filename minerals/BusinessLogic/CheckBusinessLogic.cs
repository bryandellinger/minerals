using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.BusinessLogic
{
    public class CheckBusinessLogic : ICheckBusinessLogic
    {
        private IGenericRepository<Check> genericCheckRepository;
        private ICheckRepository checkRepository;
        private IGenericRepository<File> fileRepository;

        public CheckBusinessLogic(
            IGenericRepository<Check> genericCheckRepo,
            ICheckRepository checkRepo,
            IGenericRepository<File> fileRepo
            )
        {
            genericCheckRepository = genericCheckRepo;
            checkRepository = checkRepo;
            fileRepository = fileRepo;
        }
        public object Save(Check model, long id)
        {
            if (model.Id > 0)
            {
                checkRepository.Update(model, id);
                updateFiles(model.Id, model.Files);
                return new Check { Id = model.Id };
            }
            else
            {
                List<File> files = new List<File>();
                foreach (var item in model.Files)
                {
                    files.Add(new File
                    {
                        FileGuid = item.FileGuid,
                        FileExtension = item.FileExtension,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        FileIcon = item.FileIcon
                    });
                    fileRepository.Delete(item.Id);
                }

                Check newCheck = new Check
                {
                    Id = 0,
                    CheckNum = model.CheckNum,
                    TotalAmount = model.TotalAmount,
                    Notes = model.Notes,
                    LesseeId = model.LesseeId,
                    LesseeName = model.LesseeName,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CheckDate = model.CheckDate,
                    ReceivedDate = model.ReceivedDate,
                    CreatedBy = id,
                    UpdatedBy = id,
                    Files = files
                };

                genericCheckRepository.Insert(newCheck);
                return new Check { Id = newCheck.Id };
            }
        }

        private void updateFiles(long id, IEnumerable<File> files)
        {
            foreach (var item in files)
            {
                fileRepository.Delete(item.Id);
            }
            checkRepository.UpdateFiles(id, files);
        }
    }
}
