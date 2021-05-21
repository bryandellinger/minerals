using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Minerals.ExtensionMethods;

namespace Minerals.BusinessLogic
{
    public class UploadPaymentBusinessLogic : IUploadPaymentBusinessLogic
    {
        private readonly ClientConfig clientConfig;
        private IGenericRepository<UploadPayment> genericRepository;
        private IUploadPaymentRepository repository;
        private IGenericRepository<Models.File> fileRepository;
        private IGenericRepository<Check> checkRepository;
        private IGenericRepository<UploadTemplate> uploadTemplateRepository;
        private IGenericRepository<Lessee> lesseeRepository;
        private IGenericRepository<UploadTemplateMappedHeader> uploadTemplateMappedHeaderRepository;
        private IGenericRepository<Well> wellRepository;
        private IGenericRepository<LandLeaseAgreement> landLeaseAgreementRepository;

        public UploadPaymentBusinessLogic(
             IGenericRepository<AppSetting> apRepo,
             IGenericRepository<UploadPayment> genericRepo,
             IUploadPaymentRepository repo,
             IGenericRepository<Models.File> fileRepo,
             IGenericRepository<Check> checkRepo,
             IGenericRepository<UploadTemplate> uploadTemplateRepo,
             IGenericRepository<Lessee> lesseeRepo,
             IGenericRepository<UploadTemplateMappedHeader> uploadTemplateMappedHeaderRepo,
             IGenericRepository<Well> wellRepo,
             IGenericRepository<LandLeaseAgreement> landLeaseAgreementRepo,
             ClientConfig config
            )
        {
            genericRepository = genericRepo;
            repository = repo;
            fileRepository = fileRepo;
            checkRepository = checkRepo;
            uploadTemplateRepository = uploadTemplateRepo;
            lesseeRepository = lesseeRepo;
            uploadTemplateMappedHeaderRepository = uploadTemplateMappedHeaderRepo;
            wellRepository = wellRepo;
            landLeaseAgreementRepository = landLeaseAgreementRepo;
            clientConfig = config;
        }

        public object CreateCheck(CreateCheckViewModel model, long id)
        {
            Decimal totalAmount = 0;
            UploadTemplate uploadTemplate = uploadTemplateRepository.GetById(model.UploadTemplateId);
            Lessee lessee = lesseeRepository.GetById(uploadTemplate.LesseeId);
            Models.File file = fileRepository.GetById(model.FileId);
            UploadTemplateMappedHeader uploadTemplateMappedHeader = uploadTemplateMappedHeaderRepository.GetAll()
                                                                                                                           .Where(x => x.UploadTemplateId == uploadTemplate.Id)
                                                                                                                           .Where(x => x.Attribute == Constants.AttributeTotalAmount)
                                                                                                                           .FirstOrDefault();
            if (uploadTemplateMappedHeader != null && uploadTemplateMappedHeader.Index != null)
            {
                using (OleDbConnection cn = new OleDbConnection(clientConfig.CSVConnection))
                {
                    cn.Open();
                    using (OleDbCommand cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT * FROM [{file.FileGuid.ToString()}{file.FileExtension}]";
                        cmd.CommandType = CommandType.Text;

                        using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            foreach (DbDataRecord record in reader)
                            {
                                if (!reader.IsDBNull(uploadTemplateMappedHeader.Index.Value))
                                {
                                    var value = record.GetValue(uploadTemplateMappedHeader.Index.Value).ToString();
                                    NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                                    CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                                    decimal number;
                                    if (Decimal.TryParse(value, style, culture, out number))
                                    {
                                        if(number > totalAmount)
                                        {
                                            totalAmount = number;
                                        }
                                    }                                
                                }
                            }
                        }
                    }
                }
            }

            Check check = new Check
            {
                Id = 0,
                CheckDate = model.CheckDate,
                CheckNum = model.CheckNum,
                TotalAmount = totalAmount,
                LesseeId = lessee.Id,
                LesseeName = lessee.LesseeName,
                ReceivedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                CreatedBy = id,
                UpdatedBy = id,
            };
            checkRepository.Insert(check);
            return new Check { Id = check.Id };
        }

        public object CreateCSVPayments(long fileId, long uploadTemplateId)
        {
            List<CSVPayment> csvPayments = new List<CSVPayment>();
            List<Well> wells = wellRepository.GetAll().ToList();
            UploadTemplate uploadTemplate = uploadTemplateRepository.GetById(uploadTemplateId);
            Models.File file = fileRepository.GetById(fileId);
            List<UploadTemplateMappedHeader> uploadTemplateMappedHeaders = uploadTemplateMappedHeaderRepository.GetAll()
                                                                                                                           .Where(x => x.UploadTemplateId == uploadTemplate.Id)
                                                                                                                           .Where(x => x.Index != null)
                                                                                                                           .ToList();

            using (OleDbConnection cn = new OleDbConnection(clientConfig.CSVConnection))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM [{file.FileGuid.ToString()}{file.FileExtension}]";
                    cmd.CommandType = CommandType.Text;

                    using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        long id = 0;
                        foreach (DbDataRecord record in reader)
                        {
                            Well well = FindWell(
                                    GetDataReaderString(uploadTemplateMappedHeaders, Constants.AttributeApiNum, record),
                                    GetDataReaderString(uploadTemplateMappedHeaders, Constants.AttributeAltId, record),
                                    wells);
                            DateTime? productionDate = GetDataReaderDate(uploadTemplateMappedHeaders, Constants.AttributePostMonthYear, record);
                            csvPayments.Add(new CSVPayment
                            {
                                Id = id,
                                ApiNum = well?.ApiNum,
                                WellId = well != null && well.Id > 0 ? well.Id :  (long?)null,
                                GasProd = GetDataReaderDecimal(uploadTemplateMappedHeaders, Constants.AttributeGasProd, record),
                                GasRoyalty= GetDataReaderDecimal(uploadTemplateMappedHeaders, Constants.AttributeGasRoyalty, record),
                                SalesPrice = GetDataReaderDecimal(uploadTemplateMappedHeaders, Constants.AttributeSalesPrice, record),
                                NRI = GetDataReaderDecimal(uploadTemplateMappedHeaders, Constants.AttributeNRI, record),
                                ProductionDate = productionDate,
                                PostMonth = productionDate != null ?  productionDate.Value.Month : (int?) null,
                                PostYear = productionDate != null ? productionDate.Value.Year : (int?)null,
                            });
                            id++;
                        }
                    }
                }
            }

            return csvPayments;

        }

 
        private Well FindWell(string apiNum, string altId, List<Well> wells)
        {
            Well retval = new Well();

            if (!string.IsNullOrEmpty(apiNum) && !string.IsNullOrWhiteSpace(apiNum))
            {
                retval = wells.Where(x => cleanApiNum(x.ApiNum) == cleanApiNum(apiNum)).FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(altId) && !string.IsNullOrWhiteSpace(altId))
            {
                retval = wells.Where(x => x.AltId == altId).FirstOrDefault();
            }
                return retval;
        }

        private string cleanApiNum(string apiNum)
        {
            string retval = apiNum;
            string[] apiPieces = apiNum.Split('-');
            if (apiPieces.Length > 3)
            {
                retval = $"{apiPieces[0]}-{apiPieces[1]}-{apiPieces[2]}";
            }
            return retval;

        }

        private decimal? GetDataReaderDecimal(List<UploadTemplateMappedHeader> uploadTemplateMappedHeaders, string attributeName, DbDataRecord record)
        {
            decimal? retval = null;
            UploadTemplateMappedHeader header = uploadTemplateMappedHeaders.Where(x => x.Attribute == attributeName).FirstOrDefault();
            if (header != null && header.Index != null)
            {
                var value = record.GetValue(header.Index.Value).ToString();
                NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                decimal number;
                if (Decimal.TryParse(value, style, culture, out number))
                {
                    retval = number;
                }
            }
            return retval;
        }

        private DateTime? GetDataReaderDate(List<UploadTemplateMappedHeader> uploadTemplateMappedHeaders, string attributeName, DbDataRecord record)
        {
            DateTime? retval = null;
            UploadTemplateMappedHeader header = uploadTemplateMappedHeaders.Where(x => x.Attribute == attributeName).FirstOrDefault();
            if (true)
            {
                if (header != null && header.Index != null)
                {
                    var result = record.GetValue(header.Index.Value);
                    if (result.GetType().Name == "DateTime")
                    {
                        retval = record.GetDateTime(header.Index.Value);
                    }
                }
            }
            return retval;
        }

        private string GetDataReaderString(List<UploadTemplateMappedHeader> uploadTemplateMappedHeaders, string attributeName, DbDataRecord record)
        {
            string retval = string.Empty;
            UploadTemplateMappedHeader header = uploadTemplateMappedHeaders.Where(x => x.Attribute == attributeName).FirstOrDefault();
            if (header != null && header.Index != null)
            {
                retval = record.GetValue(header.Index.Value).ToString();
            }
            return retval;
        }

        private object getValueFromFile(int index, Models.File file)
        {
            throw new NotImplementedException();
        }

        public object Save(UploadPayment model, long id)
        {
            if (model.Id > 0)
            {
                repository.DeleteCSVFiles(model.Id);
                repository.Update(model, id);
                UpdateFiles(model.Id, model.Files);
                if (model.CSVPayments.Any())
                {
                    repository.AddCSVFiles(model.Id, model.CSVPayments);
                }
    
                return new UploadTemplate { Id = model.Id };
            }
            else
            {
                List<Models.File> files = new List<Models.File>();
                foreach (var item in model.Files)
                {
                    files.Add(new Models.File
                    {
                        FileGuid = item.FileGuid,
                        FileExtension = item.FileExtension,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        FileIcon = item.FileIcon
                    });
                    var oldFile = fileRepository.GetById(item.Id);
                    if (oldFile != null)
                    {
                        fileRepository.Delete(item.Id);
                    }
                   
                }

                foreach (var item in model.CSVPayments)
                {
                    item.Id = 0;
                }

                UploadPayment newUploadPayment = new UploadPayment
                {
                    Id = 0,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CreatedBy = id,
                    UpdatedBy = id,
                    Files = files,
                    UploadTemplateId = model.UploadTemplateId,
                    UploadPaymentNotes = model.UploadPaymentNotes,
                    UploadPaymentEntryDate = model.UploadPaymentEntryDate,
                    CheckId = model.CheckId,
                    CSVPayments = model.CSVPayments
            }; 

                genericRepository.Insert(newUploadPayment);
                return new UploadTemplate { Id = newUploadPayment.Id };
            }
        }

        private void UpdateFiles(long id, IEnumerable<Models.File> files)
        {
            foreach (var item in files)
            {
                fileRepository.Delete(item.Id);
            }
            repository.UpdateFiles(id, files);
        }
    }
}
