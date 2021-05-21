using Minerals.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.BusinessLogic
{
    public class UploadTemplateBusinessLogic : IUploadTemplateBusinessLogic
    {
        private IGenericRepository<UploadTemplate> genericUploadTemplateRepository;
        private IUploadTemplateRepository uploadTemplateRepository;
        private IGenericRepository<File> fileRepository;
        private IGenericRepository<UploadTemplateUnmappedHeader> uploadTemplateUnmappedHeaderRepository;
        private IGenericRepository<UploadTemplateMappedHeader> uploadTemplateMappedHeaderRepository;

        public UploadTemplateBusinessLogic(
         IGenericRepository<UploadTemplate> genericUploadTemplateRepo,
         IUploadTemplateRepository uploadTemplateRepo,
         IGenericRepository<File> fileRepo,
         IGenericRepository<UploadTemplateUnmappedHeader> uploadTemplateUnmappedHeaderRepo,
         IGenericRepository<UploadTemplateMappedHeader> uploadTemplateMappedHeaderRepo
         )
        {
            genericUploadTemplateRepository = genericUploadTemplateRepo;
            uploadTemplateRepository = uploadTemplateRepo;
            fileRepository = fileRepo;
            uploadTemplateUnmappedHeaderRepository = uploadTemplateUnmappedHeaderRepo;
            uploadTemplateMappedHeaderRepository = uploadTemplateMappedHeaderRepo;

        }
        public object Save(UploadTemplate model, long id)
        {
            List<UploadTemplateUnmappedHeader> unmappedHeaders = new List<UploadTemplateUnmappedHeader>();
            List<UploadTemplateMappedHeader> mappedHeaders = new List<UploadTemplateMappedHeader>();

            foreach (var header in model.UploadTemplateMappedHeaders)
            {
                mappedHeaders.Add(new UploadTemplateMappedHeader
                {
                    Attribute = header.Attribute,
                    Label = header.Label,
                    Index = header.Index,
                    Header = header.Header
                });
            }

            foreach (var header in model.UploadTemplateUnmappedHeaders)
            {
                unmappedHeaders.Add(new UploadTemplateUnmappedHeader
                {
                    Header = header.Header,
                    Index = header.Index
                });
            }

            if (model.Id > 0)
            {
                uploadTemplateRepository.Update(model, id);
                updateFiles(model.Id, model.Files);
                updateHeaders(model.Id, mappedHeaders, unmappedHeaders);
                return new UploadTemplate { Id = model.Id };
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

                UploadTemplate newUploadTemplate = new UploadTemplate
                {
                    Id = 0,
                    TemplateName = model.TemplateName,
                    TemplateNotes = model.TemplateNotes,
                    LesseeId = model.LesseeId,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CreatedBy = id,
                    UpdatedBy = id,
                    Files = files,
                    UploadTemplateMappedHeaders = mappedHeaders,
                    UploadTemplateUnmappedHeaders = unmappedHeaders
                };

                genericUploadTemplateRepository.Insert(newUploadTemplate);
                return new UploadTemplate { Id = newUploadTemplate.Id };
            }
        }

        private void updateHeaders(long id, List<UploadTemplateMappedHeader> mappedHeaders, List<UploadTemplateUnmappedHeader> unmappedHeaders)
        {
            foreach (var item in uploadTemplateUnmappedHeaderRepository.GetAll().Where(x => x.UploadTemplateId == id))
            {
                uploadTemplateUnmappedHeaderRepository.Delete(item.Id);
            }
            foreach (var item in uploadTemplateMappedHeaderRepository.GetAll().Where(x => x.UploadTemplateId == id))
            {
                uploadTemplateMappedHeaderRepository.Delete(item.Id);
            }
            foreach (var item in unmappedHeaders)
            {
                uploadTemplateUnmappedHeaderRepository.Insert(new UploadTemplateUnmappedHeader
                {
                   Id =0,
                   Header = item.Header,
                   Index = item.Index,
                   UploadTemplateId = id
                });
            }
            foreach (var item in mappedHeaders)
            {
                uploadTemplateMappedHeaderRepository.Insert(new UploadTemplateMappedHeader
                {
                    Id = 0,
                    Header = item.Header,
                    Index = item.Index,
                    UploadTemplateId = id,
                    Label = item.Label,
                    Attribute = item.Attribute
                });
            }
        }

        private void updateFiles(long id, IEnumerable<File> files)
        {
            foreach (var item in files)
            {
                fileRepository.Delete(item.Id);
            }
            uploadTemplateRepository.UpdateFiles(id, files);
        }
    }
}
