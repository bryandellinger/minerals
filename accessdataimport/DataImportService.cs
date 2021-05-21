using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace accessdataimport
{
    class DataImportService : IDataImportService
    {
        private readonly string accessConnectionString;
        private readonly string townshipConnectionString;
        private readonly string fileServer;
        public enum Months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        };
        private string connectionString;

        public DataImportService()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            connectionString = configuration["DefaultSqlConnection"];

            accessConnectionString = configuration["DefaultAccessConnection"];
            townshipConnectionString = configuration["TownshipCSVConnection"];
            fileServer = configuration["FileServer"];
        }

        public void deleteData()
        {
            using var ctx = new DataContext();
            ctx.Database.SetCommandTimeout(400);
            ctx.StorageRentals.RemoveRange(ctx.StorageRentals);
            ctx.SaveChanges();
            ctx.ContractRentals.RemoveRange(ctx.ContractRentals);
            ctx.SaveChanges();
            ctx.OtherRentals.RemoveRange(ctx.OtherRentals);
            ctx.SaveChanges();
            ctx.RoyaltyAdjustments.RemoveRange(ctx.RoyaltyAdjustments);
            ctx.SaveChanges();
            ctx.Royalties.RemoveRange(ctx.Royalties);
            ctx.SaveChanges();
            ctx.WellTractInformations.RemoveRange(ctx.WellTractInformations);
            ctx.SaveChanges();
            ctx.WellOperations.RemoveRange(ctx.WellOperations);
            ctx.SaveChanges();
            ctx.CSVPayments.RemoveRange(ctx.CSVPayments);
            ctx.SaveChanges();
            ctx.Wells.RemoveRange(ctx.Wells);
            ctx.SaveChanges();
            ctx.Sureties.RemoveRange(ctx.Sureties);
            ctx.SaveChanges();
            ctx.Contracts.RemoveRange(ctx.Contracts);
            ctx.SaveChanges();
            ctx.Files.RemoveRange(ctx.Files);
            ctx.SaveChanges();
            ctx.WellBoreShares.RemoveRange(ctx.WellBoreShares);
            ctx.SaveChanges();
            Console.WriteLine("--deleted royalties checks and adjustments");
            ctx.RevenueReceived.RemoveRange(ctx.RevenueReceived);
            ctx.SaveChanges();
            ctx.LesseeContacts.RemoveRange(ctx.LesseeContacts);
            ctx.SaveChanges();
            ctx.LesseeHistories.RemoveRange(ctx.LesseeHistories);
            ctx.SaveChanges();
            ctx.ContractEventDetails.RemoveRange(ctx.ContractEventDetails);
            ctx.SaveChanges();
            ctx.TractLesseeJunctions.RemoveRange(ctx.TractLesseeJunctions);
            ctx.SaveChanges();
            ctx.Pads.RemoveRange(ctx.Pads);
            ctx.SaveChanges();
            ctx.Users.RemoveRange(ctx.Users);
            ctx.SaveChanges();
            ctx.Roles.RemoveRange(ctx.Roles);
            ctx.SaveChanges();
            ctx.UserRoleJunctions.RemoveRange(ctx.UserRoleJunctions);
            ctx.SaveChanges();
            ctx.SurfaceUseAgreements.RemoveRange(ctx.SurfaceUseAgreements);
            ctx.SaveChanges();
            ctx.AdditionalBonuses.RemoveRange(ctx.AdditionalBonuses);
            ctx.SaveChanges();
            ctx.PaymentRequirements.RemoveRange(ctx.PaymentRequirements);
            ctx.SaveChanges();
            ctx.ProspectingAgreements.RemoveRange(ctx.ProspectingAgreements);
            ctx.SaveChanges();
            ctx.ProductionAgreements.RemoveRange(ctx.ProductionAgreements);
            ctx.SaveChanges();
            ctx.SeismicAgreements.RemoveRange(ctx.SeismicAgreements);
            ctx.SaveChanges();
            ctx.AssociatedContracts.RemoveRange(ctx.AssociatedContracts);
            ctx.SaveChanges();
            ctx.AssociatedTracts.RemoveRange(ctx.AssociatedTracts);
            ctx.SaveChanges();
            ctx.ContractSubTypes.RemoveRange(ctx.ContractSubTypes);
            ctx.SaveChanges();
            ctx.ContractTypes.RemoveRange(ctx.ContractTypes);
            ctx.SaveChanges();
            ctx.Tracts.RemoveRange(ctx.Tracts);
            ctx.SaveChanges();
            ctx.Lessees.RemoveRange(ctx.Lessees);
            ctx.SaveChanges();
            ctx.Districts.RemoveRange(ctx.Districts);
            ctx.SaveChanges();
            ctx.DistrictContractJunctions.RemoveRange(ctx.DistrictContractJunctions);
            ctx.SaveChanges();
            ctx.DistrictTractJunctions.RemoveRange(ctx.DistrictTractJunctions);
            ctx.SaveChanges();
            ctx.TownshipLandLeaseAgreementJunctions.RemoveRange(ctx.TownshipLandLeaseAgreementJunctions);
            ctx.SaveChanges();
            ctx.TownshipSurfaceUseAgreementJunctions.RemoveRange(ctx.TownshipSurfaceUseAgreementJunctions);
            ctx.SaveChanges();
            ctx.TownshipProspectingAgreementJunctions.RemoveRange(ctx.TownshipProspectingAgreementJunctions);
            ctx.SaveChanges();
            ctx.TownshipProductionAgreementJunctions.RemoveRange(ctx.TownshipProductionAgreementJunctions);
            ctx.SaveChanges();
            ctx.TownshipSeismicAgreementJunctions.RemoveRange(ctx.TownshipSeismicAgreementJunctions);
            ctx.SaveChanges();
            ctx.TractUnitJunctionWellJunctions.RemoveRange(ctx.TractUnitJunctionWellJunctions);
            ctx.SaveChanges();
            ctx.TractUnitJunctions.RemoveRange(ctx.TractUnitJunctions);
            ctx.SaveChanges();
            ctx.TractLesseeJunctions.RemoveRange(ctx.TractLesseeJunctions);
            ctx.SaveChanges();
            ctx.LandLeaseAgreements.RemoveRange(ctx.LandLeaseAgreements);
            ctx.SaveChanges();
            ctx.Townships.RemoveRange(ctx.Townships);
            ctx.SaveChanges();
            ctx.TerminationReasons.RemoveRange(ctx.TerminationReasons);
            ctx.SaveChanges();
            ctx.AltIdCategories.RemoveRange(ctx.AltIdCategories);
            ctx.SaveChanges();
            ctx.RowContracts.RemoveRange(ctx.RowContracts);
            ctx.SaveChanges();
            ctx.PluggingSuretyDetails.RemoveRange(ctx.PluggingSuretyDetails);
            ctx.SaveChanges();
            ctx.States.RemoveRange(ctx.States);
            ctx.SaveChanges();
            ctx.ContractEventDetailReasonsForChange.RemoveRange(ctx.ContractEventDetailReasonsForChange);
            ctx.SaveChanges();
            ctx.WellStatuses.RemoveRange(ctx.WellStatuses);
            ctx.SaveChanges();
            ctx.WellTypes.RemoveRange(ctx.WellTypes);
            ctx.SaveChanges();
            ctx.ApiCodes.RemoveRange(ctx.ApiCodes);
            ctx.SaveChanges();
            ctx.Units.RemoveRange(ctx.Units);
            ctx.SaveChanges();
            ctx.Formations.RemoveRange(ctx.Formations);
            ctx.SaveChanges();
            ctx.DigitalImageWellLogTestTypeWellJunctions.RemoveRange(ctx.DigitalImageWellLogTestTypeWellJunctions);
            ctx.SaveChanges();
            ctx.DigitalWellLogTestTypeWellJunctions.RemoveRange(ctx.DigitalWellLogTestTypeWellJunctions);
            ctx.SaveChanges();
            ctx.HardCopyWellLogTestTypeWellJunctions.RemoveRange(ctx.HardCopyWellLogTestTypeWellJunctions);
            ctx.SaveChanges();
            ctx.WellLogTestTypes.RemoveRange(ctx.WellLogTestTypes);
            ctx.SaveChanges();
            ctx.AppSettings.RemoveRange(ctx.AppSettings);
            ctx.SaveChanges();
            ctx.PaymentTypes.RemoveRange(ctx.PaymentTypes);
            ctx.SaveChanges();
            ctx.ProductTypes.RemoveRange(ctx.ProductTypes);
            ctx.SaveChanges();
            ctx.StorageWellPaymentMonthJunctions.RemoveRange(ctx.StorageWellPaymentMonthJunctions);
            ctx.SaveChanges();
            ctx.StorageBaseRentalPaymentMonthJunctions.RemoveRange(ctx.StorageBaseRentalPaymentMonthJunctions);
            ctx.SaveChanges();
            ctx.StorageRentalPaymentMonthJunctions.RemoveRange(ctx.StorageRentalPaymentMonthJunctions);
            ctx.SaveChanges();
            ctx.ContractRentalPaymentMonthJunctions.RemoveRange(ctx.ContractRentalPaymentMonthJunctions);
            ctx.SaveChanges();
            ctx.Storages.RemoveRange(ctx.Storages);
            ctx.SaveChanges();
            ctx.Months.RemoveRange(ctx.Months);
            ctx.SaveChanges();
            ctx.Checks.RemoveRange(ctx.Checks);
            ctx.SaveChanges();
            ctx.StorageRentalPaymentTypes.RemoveRange(ctx.StorageRentalPaymentTypes);
            ctx.SaveChanges();
            ctx.StorageBaseRentalPaymentMonthJunctions.RemoveRange(ctx.StorageBaseRentalPaymentMonthJunctions);
            ctx.SaveChanges();
            ctx.StorageRentalPaymentMonthJunctions.RemoveRange(ctx.StorageRentalPaymentMonthJunctions);
            ctx.SaveChanges();
            ctx.StorageWellPaymentMonthJunctions.RemoveRange(ctx.StorageWellPaymentMonthJunctions);
            ctx.SaveChanges();
            ctx.PeriodTypes.RemoveRange(ctx.PeriodTypes);
            ctx.SaveChanges();
            ctx.UploadTemplates.RemoveRange(ctx.UploadTemplates);
            ctx.SaveChanges();
            ctx.UploadTemplateMappedHeaders.RemoveRange(ctx.UploadTemplateMappedHeaders);
            ctx.SaveChanges();
            ctx.UploadTemplateUnmappedHeaders.RemoveRange(ctx.UploadTemplateUnmappedHeaders);
            ctx.SaveChanges();
            ctx.UploadPayments.RemoveRange(ctx.UploadPayments);
            ctx.SaveChanges();
            ctx.AdditionalContractInformations.RemoveRange(ctx.AdditionalContractInformations);
            ctx.SaveChanges();
            ctx.BondCategories.RemoveRange(ctx.BondCategories);
            ctx.SaveChanges();
            ctx.SuretyTypes.RemoveRange(ctx.SuretyTypes);
            ctx.SaveChanges();
            ctx.RiderReasons.RemoveRange(ctx.RiderReasons);
            ctx.SaveChanges();
            ctx.SuretyRiders.RemoveRange(ctx.SuretyRiders);
            ctx.SaveChanges();
            ctx.SuretyWells.RemoveRange(ctx.SuretyWells);
            ctx.SaveChanges();
        }

        public List<Unit> GetUnits()
        {
            List<Unit> retval = new List<Unit>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblUnit", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {

                    retval.Add(new Unit
                    {
                        Id = 0,
                        UnitName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("UnitName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("UnitName")),
                        PKUnitId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkUnitID")),
                        DPUAcres = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("UnitAcres")) ? (Double?)null : DB_Reader.GetDouble(DB_Reader.GetOrdinal("UnitAcres")),
                        DPUAcresEffectiveDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("UnitEffectiveDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("UnitEffectiveDate")),
                    });
                }
            }
            return retval;
        }

        public List<Lessee> GetLessees()
        {
            List<Lessee> retval = new List<Lessee>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tbluLessee", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    long pkLesseeId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkLesseeID"));
                    List<LesseeContact> lesseeContacts = getLesseeContacts(pkLesseeId);
                    retval.Add(new Lessee
                    {
                        Id = 0,
                        LogicalDeleteIn = false,
                        LesseeName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LesseeName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("LesseeName")),
                        PKLesseeId = pkLesseeId,
                        LesseeContacts = lesseeContacts
                    });
                }
            }

            return retval;
        }

        private List<LesseeContact> getLesseeContacts(long pkLesseeId)
        {
            List<LesseeContact> lesseeContacts = new List<LesseeContact>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblContact WHERE fkLesseeID = ?", con))
            {
                Command.Parameters.AddWithValue("fkLesseeID", Convert.ToInt32(pkLesseeId));
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    lesseeContacts.Add(new LesseeContact
                    {
                        Id = 0,
                        LastName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("LName")),
                        FirstName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("FName")),
                        Title = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Title")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Title")),
                        Address1 = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Address1")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Address1")),
                        Address2 = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Address2")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Address2")),
                        City = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("City")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("City")),
                        State = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("State")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("State")),
                        Zip = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Zip")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Zip")),
                        Phone1 = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Phone1")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Phone1")),
                        Phone2 = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Phone2")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Phone2")),
                        Fax = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Fax")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Fax")),
                        Email = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Email")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Email")),
                        Notes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                    });
                }
            }
            return lesseeContacts;
        }

        public List<RevenueReceived> GetRevenueReceived(List<Lessee> lessees)
        {
            List<RevenueReceived> retval = new List<RevenueReceived>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from tblRevenueReceived", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    string lesseeName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Company")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Company"));
                    Lessee lessee = lessees.FirstOrDefault(x => x.LesseeName == lesseeName);

                    var r = new RevenueReceived
                    {
                        Id = 0,
                        PKCheckId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkCheckId")),
                        CheckNum = DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum")),
                        RecvDate = DB_Reader.GetDateTime(DB_Reader.GetOrdinal("RecvDate")),
                        CheckDate = DB_Reader.GetDateTime(DB_Reader.GetOrdinal("CheckDate")),
                        RecvCheckTotal = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RecvCheckTotal")) ? 0 : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("RecvCheckTotal")),
                        RTNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RTNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("RTNum")),
                        ReverseTransDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RevTransDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("RevTransDate")),
                        DocNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("DocNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("DocNum")),
                        PostedDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PostedDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("PostedDate")),
                        Notes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                        ControlNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ControlNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("ControlNum")),
                        CheckReturned = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckReturned")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("CheckReturned")),
                        FloorPriceAudit = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("FloorPriceAudit")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("FloorPriceAudit")),
                        NRIAudit = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("NRIAudit")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("NRIAudit")),
                        Over90DayAudit = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Over90DayAudit")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("Over90DayAudit")),
                    };

                    if (lessee != null)
                    {
                        r.LesseeId = lessee.Id;
                    }
                    retval.Add(r);
                }
            }
            return retval;
        }

        public List<Tract> GetTracts(List<District> districts, List<Lessee> lessees)
        {
            List<ContractType> contractTypes = new List<ContractType>();
            List<ContractSubType> contractSubTypes = new List<ContractSubType>();

            using (var ctx = new DataContext())
            {
                contractTypes = ctx.ContractTypes.ToList();
                contractSubTypes = ctx.ContractSubTypes.ToList();
            }

            List<Tract> retval = new List<Tract>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblTract", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    Lessee lessee = new Lessee();
                    District district = new District();
                    long? fkDistrictID = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkDistrictID")) ? (int?)null : DB_Reader.GetInt16(DB_Reader.GetOrdinal("fkDistrictID"));
                    long? fkLesseeId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkLesseeOfRecord")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeOfRecord"));
                    long? districtId = null;
                    long? lesseeId = null;

                    if (fkDistrictID != null)
                    {
                        district = districts.FirstOrDefault(x => x.DistrictId == fkDistrictID.Value);
                        districtId = district?.Id;
                    }

                    if (fkLesseeId != null)
                    {
                        lessee = lessees.FirstOrDefault(x => x.PKLesseeId == fkLesseeId.Value);
                        lesseeId = lessee?.Id;
                    }

                    var tract = new Tract
                    {
                        Id = 0,
                        PKTractId = DB_Reader.GetString(DB_Reader.GetOrdinal("pkTractId")),
                        TractNum = DB_Reader.GetString(DB_Reader.GetOrdinal("pkTractId")),
                        ContractNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ContractNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("ContractNum")),
                        LeaseDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LeaseDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("LeaseDate")),
                        Acreage = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Acreage")) ? (Decimal?)null : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("Acreage")),
                        RoyaltyPercent = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RoyaltyPercent")) ? (Decimal?)null : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("RoyaltyPercent")),
                        RoyaltyFlDollar = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RoyaltyFlDollar")) ? (Single?)null : DB_Reader.GetFloat(DB_Reader.GetOrdinal("RoyaltyFlDollar")),
                        Rent = GetDecimalFromCurrency(DB_Reader, DB_Reader.GetOrdinal("Rent")),
                        Administrative = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Administrative")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Administrative")),
                        Notes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                        TerminatedDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("TerminatedDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("TerminatedDate")),
                        Terminated = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Terminated")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Terminated")),
                        IncreasedRentAmnt = GetDecimalFromCurrency(DB_Reader, DB_Reader.GetOrdinal("IncreasedRentAmnt")),
                        YearIncRentEffec = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("YearIncRentEffec")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("YearIncRentEffec")),
                        RentAsOfToday = GetDecimalFromCurrency(DB_Reader, DB_Reader.GetOrdinal("RentAsOfToday")),
                        NotesAdditional = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("NotesAdditional")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("NotesAdditional"))),
                        RiverTract = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RiverTract")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("RiverTract")),
                        BondsRequired = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("BondsRequired")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("BondsRequired")),
                        RentalMonthDue = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RentalMonthDue")) ? (DateTime?)null : GetDateTimeFromRentalMonthDue(DB_Reader.GetString(DB_Reader.GetOrdinal("RentalMonthDue"))),
                        RentalAnnivDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RentalAnnivDate")) ? (DateTime?)null : GetDateTimeFromRentalAnnivDate(DB_Reader.GetString(DB_Reader.GetOrdinal("RentalAnnivDate"))),
                        RentalsRequired = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RentalsRequired")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("RentalsRequired")),
                        Horizon = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Horizon")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Horizon")),
                        StateParkName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("StateParkName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("StateParkName")),
                        LandIncluded = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LandIncluded")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("LandIncluded")),
                        ReversionDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ReversionDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("ReversionDate")),
                        DistrictId = districtId,
                        LesseeId = lesseeId,
                    };
                    retval.Add(tract);
                }
            }
            return retval;
        }

        private long? GetContractSubTypeId(string contractNum, List<ContractSubType> contractSubTypes, string landIncluded)
        {
            long? retval = null;

            if (contractNum.StartsWith("M-1"))
            {
                retval = contractSubTypes.FirstOrDefault(x => x.ContractSubTypeName == "State Park")?.Id;
                if (landIncluded == "State Forest Land Only")
                {
                    retval = contractSubTypes.FirstOrDefault(x => x.ContractSubTypeName == "State Forest")?.Id;
                }
                if (landIncluded == "State Forest & State Park")
                {
                    retval = contractSubTypes.FirstOrDefault(x => x.ContractSubTypeName == "State Forest & State Park")?.Id;
                }

            }
            if (contractNum.StartsWith("M-120"))
            {
                retval = contractSubTypes.FirstOrDefault(x => x.ContractSubTypeName == "Gas Storage Field")?.Id;
            }
            if (contractNum.StartsWith("M-2"))
            {
                retval = contractSubTypes.FirstOrDefault(x => x.ContractSubTypeName == "Streambed")?.Id;
            }
            if (contractNum.StartsWith("M-3"))
            {
                retval = contractSubTypes.FirstOrDefault(x => x.ContractSubTypeName == "DGS Land")?.Id;
            }
            if (contractNum.StartsWith("M-60"))
            {
                retval = contractSubTypes.FirstOrDefault(x => x.ContractSubTypeName == "Oil and Gas")?.Id;
            }
            if (contractNum.StartsWith("M-62"))
            {
                retval = contractSubTypes.FirstOrDefault(x => x.ContractSubTypeName == "Coal")?.Id;
            }

            return retval;
        }

        private long? GetContractTypeId(string contractNum, List<ContractType> contractTypes)
        {
            long? retval = null;
            if (contractNum.StartsWith("M-1") || contractNum.StartsWith("M-2") || contractNum.StartsWith("M-3"))
            {
                retval = contractTypes.FirstOrDefault(x => x.ContractTypeName == "Land Lease")?.Id;
            }
            if (contractNum.StartsWith("M-4"))
            {
                retval = contractTypes.FirstOrDefault(x => x.ContractTypeName == "Prospecting Agreement")?.Id;
            }
            if (contractNum.StartsWith("M-5"))
            {
                retval = contractTypes.FirstOrDefault(x => x.ContractTypeName == "Production Agreement")?.Id;
            }
            if (contractNum.StartsWith("M-6"))
            {
                retval = contractTypes.FirstOrDefault(x => x.ContractTypeName == "SUA")?.Id;
            }
            return retval;
        }


        private DateTime? GetDateTimeFromRentalMonthDue(string v)
        {
            try
            {
                string[] datePieces = v.Split('-');
                string yearAsString = datePieces[0].Trim();
                string monthAsString = datePieces[1].Trim();
                int twoDigitYear = 0;
                Int32.TryParse(yearAsString, out twoDigitYear);
                int fourDigitYear = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(twoDigitYear);
                int month = (int)(Months)Enum.Parse(typeof(Months), monthAsString);
                return new DateTime(fourDigitYear, month, 1);
            }
            catch (Exception)
            {

                return (DateTime?)null;
            }

        }

        private DateTime? GetDateTimeFromRentalAnnivDate(string v)
        {
            try
            {
                string[] datePieces = v.Split('/');
                string yearAsString = datePieces[1].Trim();
                string monthAsString = datePieces[0].Trim();
                int twoDigitYear = 0;
                int month = 0;
                Int32.TryParse(yearAsString, out twoDigitYear);
                int fourDigitYear = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(twoDigitYear);
                Int32.TryParse(monthAsString, out month);
                return new DateTime(fourDigitYear, month, 1);
            }
            catch (Exception)
            {

                return (DateTime?)null;
            }

        }

        private decimal? GetDecimalFromCurrency(OleDbDataReader dB_Reader, int v)
        {
            if (dB_Reader.IsDBNull(v))
            {
                return (Decimal?)null;
            }
            else
            {
                try
                {
                    return dB_Reader.GetDecimal(v);
                }
                catch (Exception)
                {

                    return (Decimal?)null;
                }
            }
        }

        public List<Lessee> InsertLessees(List<Lessee> lessees)
        {
            using var ctx = new DataContext();
            ctx.Lessees.AddRange(lessees);
            ctx.SaveChanges();
            return lessees;
        }

        public List<Unit> InsertUnits(List<Unit> units)
        {
            var distinctunits = units.Where(x => x.UnitName != null).GroupBy(x => x.UnitName).Select(x => x.FirstOrDefault());
            foreach (var item in distinctunits)
            {
                item.IsActiveInd = true;
                item.UnitGroup = new UnitGroup { Id = 0 };
            }
            using var ctx = new DataContext();
            ctx.Units.AddRange(distinctunits);
            ctx.SaveChanges();
            return units;
        }

        public List<RevenueReceived> InsertRevenuesReceived(List<RevenueReceived> revenuesReceived)
        {
            using var ctx = new DataContext();
            ctx.RevenueReceived.AddRange(revenuesReceived);
            ctx.SaveChanges();
            return revenuesReceived;
        }

        public List<Tract> InsertTracts(List<Tract> tracts)
        {
            using var ctx = new DataContext();
            ctx.Tracts.AddRange(tracts);
            ctx.SaveChanges();
            return tracts;
        }

        public List<DistrictTractJunction> InsertDistrictTractJunctions(List<Tract> tracts)
        {
            List<DistrictTractJunction> districtTractJunctions = new List<DistrictTractJunction>();
            foreach (var tract in tracts)
            {
                if (tract.DistrictId != null)
                {
                    districtTractJunctions.Add(new DistrictTractJunction { Id = 0, DistrictId = tract.DistrictId.Value, TractId = tract.Id });
                }
            }
            using var ctx = new DataContext();
            ctx.DistrictTractJunctions.AddRange(districtTractJunctions);
            ctx.SaveChanges();
            return districtTractJunctions;
        }


        public List<TractLesseeJunction> GetTractLesseeJunctions(List<Tract> insertedTracts, List<Lessee> insertedLeeses)
        {
            List<TractLesseeJunction> retval = new List<TractLesseeJunction>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblkTractLessee", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    var FKLesseeId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeId"));
                    var FKTractId = DB_Reader.GetString(DB_Reader.GetOrdinal("fkTractID"));
                    var lessee = insertedLeeses.FirstOrDefault(x => x.PKLesseeId == FKLesseeId);
                    var tract = insertedTracts.FirstOrDefault(x => x.PKTractId == FKTractId);
                    if (tract?.Id != null && lessee?.Id != null)
                    {
                        TractLesseeJunction tractLesseeJunction = new TractLesseeJunction
                        {
                            Id = 0,
                            LesseeId = lessee.Id,
                            TractId = tract.Id
                        };
                        retval.Add(tractLesseeJunction);
                    }
                }
            }
            return retval;
        }

        public List<TractLesseeJunction> InsertTractLesseeJuntions(List<TractLesseeJunction> tractLesseeJunctions)
        {
            using var ctx = new DataContext();
            ctx.TractLesseeJunctions.AddRange(tractLesseeJunctions);
            ctx.SaveChanges();
            return tractLesseeJunctions;
        }

        public List<Pad> GetPads(List<Tract> insertedTracts)
        {
            List<Pad> retval = new List<Pad>();

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblPad", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    var fkTractId = DB_Reader.GetString(DB_Reader.GetOrdinal("fkTractId"));
                    var tract = insertedTracts.FirstOrDefault(x => x.PKTractId == fkTractId);

                    if (tract?.Id != null)
                    {
                        retval.Add(new Pad
                        {
                            Id = 0,
                            PKPadId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkPadId")),
                            TractId = tract.Id,
                            PadName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PadName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("PadName")),
                        });
                    }
                }
            }
            return retval;
        }



        public List<Pad> InsertPads(List<Pad> pads)
        {
            using var ctx = new DataContext();
            ctx.Pads.AddRange(pads);
            ctx.SaveChanges();
            return pads;
        }

        public List<Well> GetWells(
            List<Pad> insertedPads,
            List<WellStatus> wellStatuses,
            List<WellStatusLookup> wellStatusLookups,
            List<Lessee> insertedLessees,
            List<WellOperator> wellOperators,
            List<AltContractExcel> altContractExcelList,
            List<AltIdCategory> altIdCategories,
            List<Formation> formations,
            List<NonUnitizedWellsAcre> nonUnitizedWellsAcres)
        {
            List<Well> retval = new List<Well>();

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblWell", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    var fkPadId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkPadId")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkPadId"));
                    var pad = insertedPads.FirstOrDefault(x => x.PKPadId == fkPadId);
                    var accessName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Status")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Status"));
                    var wellStatusLookup = wellStatusLookups.FirstOrDefault(x => x.AccessName == accessName);
                    var apiNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ApiNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("ApiNum"));
                    double acreageAttributableToWells = 0;
                    if (!string.IsNullOrEmpty(apiNum))
                    {
                        var nonUnitizedWellsAcre = nonUnitizedWellsAcres.Where(x => x.ApiNum == apiNum).FirstOrDefault();
                        if (nonUnitizedWellsAcre != null)
                        {
                            acreageAttributableToWells = nonUnitizedWellsAcre.AcreageAttributableToWells;
                        }
                    }
                    string formationName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Formation")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Formation"));
                    long? producingFormationId = null;
                    var formation = formations.FirstOrDefault(x => x.FormationName == formationName);
                    if (formation != null)
                    {
                        producingFormationId = formation.Id;
                    }
                    long? lesseeId = null;
                    var wellOperator = wellOperators.FirstOrDefault(x => x.ApiNum == apiNum);
                    if (wellOperator != null)
                    {
                        var lessee = insertedLessees.FirstOrDefault(x => x.LesseeName == wellOperator.Operator);
                        if (lessee != null)
                        {
                            lesseeId = lessee.Id;
                        }
                    }

                    string altId = null;
                    string altIdType = null;
                    var altContract = altContractExcelList.FirstOrDefault(x => x.ApiNum == apiNum);
                    if (altContract != null)
                    {
                        altId = altContract.AltId;
                        altIdType = "Operator ID";
                    }


                    if (wellStatusLookup != null)
                    {
                        if (pad?.Id != null)
                        {
                            retval.Add(new Well
                            {
                                Id = 0,
                                PKWellId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkWellID")),
                                FkUnitId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkUnitID")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkUnitID")),
                                PadId = pad.Id,
                                WellNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("WellNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("WellNum")),
                                ApiNum = apiNum,
                                LesseeId = lesseeId,
                                WellCode = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("WellCode")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("WellCode")),
                                VDepth = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("VDepth")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("VDepth")),
                                HDepth = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("HDepth")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("HDepth")),
                                Drainage = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Drainage")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("Drainage")),
                                BofAppDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("BofAppDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("BofAppDate")),
                                PermitDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PermitDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("PermitDate")),
                                SpudDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("SpudDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("SpudDate")),
                                PlugDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PlugDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("PlugDate")),
                                WellStatusId = wellStatuses.First(x => x.WellStatusName == wellStatusLookup.WellStatusName).Id,
                                Lat = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Lat")) ? (Double?)null : DB_Reader.GetDouble(DB_Reader.GetOrdinal("Lat")),
                                Long = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Long")) ? (Double?)null : DB_Reader.GetDouble(DB_Reader.GetOrdinal("Long")),
                                Notes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                                WellLocation = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("WellLocation")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("WellLocation")),
                                RoyaltyPercentOverride = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RoyaltyPercentOverride")) ? (Decimal?)null : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("RoyaltyPercentOverride")),
                                Horizon = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Horizon")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Horizon")),
                                Severed = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Severed")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Severed")),
                                Elevation = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Elevation")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("Elevation")),
                                AltId = altId,
                                AltIdType = altIdType,
                                ProducingFormationId = producingFormationId,
                                AutoUpdatedAllowedInd = true,
                                WellboreLengthInd = false,
                                TotalBoreholeLengthOverrideInd = false,
                                AcreageAttributableToWells = acreageAttributableToWells
                            });
                        }
                    }
                }
            }

            return retval;
        }

        public List<Well> InsertWells(List<Well> wells)
        {
            using var ctx = new DataContext();
            ctx.Wells.AddRange(wells);
            ctx.SaveChanges();
            return wells;
        }

        public List<WellTractInformation> InsertWellTractInformation(List<Well> wells,
            List<WellOperation> wellOperations,
            List<Lessee> lessees,
            List<Tract> tracts)
        {
            using var ctx = new DataContext();
            var pads = ctx.Pads.ToList();
            var contracts = ctx.Contracts.ToList();
            var contractEventDetails = ctx.ContractEventDetails.Where(x => x.ActiveInd == true).ToList();
            List<WellTractInformation> wellTractInformations = new List<WellTractInformation>();
            foreach (var item in wellOperations)
            {
                Well well = wells.FirstOrDefault(x => x.Id == item.WellId);
                if (well != null)
                {
                    long? contractId = null;
                    long? tractId = null;
                    decimal? royaltyPercent = null;
                    var lessee = lessees.FirstOrDefault(x => x.Id == item.LesseeId);

                    var pad = pads.FirstOrDefault(x => x.Id == well.PadId);
                    if (pad != null)
                    {
                        tractId = pad.TractId;
                        var contract = contracts.FirstOrDefault(x => x.TractId == pad.TractId);
                        var tract = tracts.FirstOrDefault(x => x.Id == pad.TractId);
                        if (tract != null && tract.RoyaltyPercent != null)
                        {
                            royaltyPercent = tract.RoyaltyPercent;
                        }
                        if (contract != null)
                        {
                            contractId = contract.Id;
                        }

                    }
                    if (tractId == null && contractId != null)
                    {
                        var c = contracts.FirstOrDefault(x => x.Id == contractId);
                        if (c != null && c.TractId != null)
                        {
                            tractId = c.TractId;
                            if (royaltyPercent == null)
                            {
                                var tract = tracts.FirstOrDefault(x => x.Id == pad.TractId);
                                if (tract != null && tract.RoyaltyPercent != null)
                                {
                                    royaltyPercent = tract.RoyaltyPercent;
                                }
                            }
                        }
                    }
                    if (lessee != null && item.WellId != null)
                    {
                        wellTractInformations.Add(new WellTractInformation
                        {
                            Id = 0,
                            WellOperationId = item.Id,
                            PKWellOpsId = item.PKWellOpsId,
                            WellId = item.WellId.Value,
                            LesseeId = item.LesseeId,
                            LesseeName = lessee.LesseeName,
                            RoyaltyPercent = royaltyPercent,
                            PadId = well.PadId,
                            TractId = tractId,
                            ContractId = contractId,
                            ActiveInd = true,
                            PercentOwnership = item.OpShare == null ? (decimal?)null : Convert.ToDecimal(item.OpShare.Value)
                        });
                    }
                }
            }

            foreach (var well in from c in wells
                                 where !(from o in wellTractInformations
                                         select o.WellId)
                                        .Contains(c.Id)
                                 select c)
            {
                long? contractId = null;
                long? tractId = null;
                decimal? royaltyPercent = null;

                var pad = pads.FirstOrDefault(x => x.Id == well.PadId);
                if (pad != null)
                {
                    tractId = pad.TractId;
                    var tract = tracts.FirstOrDefault(x => x.Id == pad.TractId);
                    if (tract != null && tract.RoyaltyPercent != null)
                    {
                        royaltyPercent = tract.RoyaltyPercent;
                    }

                    var contract = contracts.FirstOrDefault(x => x.TractId == pad.TractId);
                    if (contract != null)
                    {
                        contractId = contract.Id;
                    }

                }
                if (tractId == null && contractId != null)
                {
                    var c = contracts.FirstOrDefault(x => x.Id == contractId);
                    if (c != null && c.TractId != null)
                    {
                        tractId = c.TractId;
                        var tract = tracts.FirstOrDefault(x => x.Id == pad.TractId);
                        if (tract != null && tract.RoyaltyPercent != null)
                        {
                            royaltyPercent = tract.RoyaltyPercent;
                        }
                    }
                }

                var contractEventDetailsByContract = contractEventDetails.Where(x => x.ContractId == contractId).ToList();
                if (contractEventDetailsByContract.Any())
                {
                    foreach (var item in contractEventDetailsByContract)
                    {
                        wellTractInformations.Add(new WellTractInformation
                        {
                            Id = 0,
                            WellId = well.Id,
                            PadId = well.PadId,
                            TractId = tractId,
                            ContractId = contractId,
                            ActiveInd = true,
                            LesseeId = item.LesseeId,
                            LesseeName = item.LesseeName,
                            PercentOwnership = item.ShareOfLeasePercentage,
                            RoyaltyPercent = royaltyPercent,
                        });
                    }
                }
                else
                {
                    wellTractInformations.Add(new WellTractInformation
                    {
                        Id = 0,
                        WellId = well.Id,
                        PadId = well.PadId,
                        TractId = tractId,
                        ContractId = contractId,
                        ActiveInd = true,
                        RoyaltyPercent = royaltyPercent
                    });
                }

            }
            ctx.WellTractInformations.AddRange(wellTractInformations);
            ctx.SaveChanges();
            return wellTractInformations;
        }

        public List<RoyaltyAdjustment> GetRoyaltyAdustments(List<Royalty> insertedroyalties, List<Check> insertedChecks)
        {
            List<RoyaltyAdjustment> retval = new List<RoyaltyAdjustment>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT  * from dbo_tblRoyAdjust", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    long fkRoyaltyId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkRoyaltyId"));
                    long? fkCheckId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkCheckId")) ? (long?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkCheckId"));
                    string checkNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum"));
                    Royalty royalty = insertedroyalties.Where(x => x.PKRoyaltyId == fkRoyaltyId).FirstOrDefault();
                    Check check = new Check();
                    if (fkCheckId != null)
                    {

                        check = insertedChecks.Where(x => x.PKCheckId == fkCheckId.Value).FirstOrDefault();
                    }

                    if (check == null && !string.IsNullOrEmpty(checkNum))
                    {
                        check = insertedChecks.Where(x => x.CheckNum == checkNum).FirstOrDefault();
                    }
                    if (royalty != null && check != null)
                    {
                        retval.Add(new RoyaltyAdjustment
                        {
                            Id = 0,
                            RoyaltyId = royalty.Id,
                            GasProd = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("GasProd")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("GasProd")),
                            OilProd = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("OilProd")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("OilProd")),
                            GasRoyalty = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("GasRoyalty")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("GasRoyalty")),
                            OilRoyalty = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("OilRoyalty")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("OilRoyalty")),
                            SalesPrice = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("SalesPrice")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("SalesPrice")),
                            CompressDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CompressDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("CompressDeduction")),
                            TransDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("TransDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("TransDeduction")),
                            Deduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Deduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("Deduction")),
                            Flaring = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Flaring")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Flaring")),
                            NRI = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("NRI")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("NRI")),
                            EntryDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EntryDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EntryDate")),
                            CheckId = check != null ? check.Id : (long?)null
                        });
                    }
                }

                return retval;
            }
        }


        public List<Storage> GetStorages(List<Lessee> insertedLeeses, List<StorageMonthLookup> storageMonthLookups)
        {
            List<StorageRental> storageRentals = new List<StorageRental>();
            List<Storage> retval = new List<Storage>();
            using var ctx = new DataContext();

            List<Check> checks = ctx.Checks.ToList();
            var contractSubType = ctx.ContractSubTypes.Where(x => x.ContractSubTypeName == "Gas Storage Field").First();
            var landLeaseAgreements = ctx.LandLeaseAgreements.Include(x => x.Contract).Where(x => x.Contract.ContractSubTypeId == contractSubType.Id).ToList();

            List<Month> months = ctx.Months.ToList();
            List<StorageRentalPaymentType> storageRentalPaymentTypes = ctx.StorageRentalPaymentTypes.ToList();
            List<PeriodType> periodTypes = ctx.PeriodTypes.ToList();

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT  * from dbo_tblStorRent", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    var checkNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("checkNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum"));
                    var check = checks.FirstOrDefault(x => !string.IsNullOrEmpty(checkNum) && x.CheckNum == checkNum);
                    if (check != null)
                    {
                        string PeriodType = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PeriodType")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("PeriodType"));
                        long? periodTypeId = null;
                        PeriodType periodType = periodTypes.FirstOrDefault(x => x.PeriodTypeName == PeriodType);
                        if (periodType != null)
                        {
                            periodTypeId = periodType.Id;
                        }
                        string PaymentType = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PaymentType")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("PaymentType"));

                        string paymentTypeFromAccess = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PaymentType")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("PaymentType"));
                        string well = null;

                        StorageRentalPaymentType storageRentalPaymentType = storageRentalPaymentTypes.FirstOrDefault(x => x.StorageRentalPaymentTypeName == paymentTypeFromAccess);
                        if (storageRentalPaymentType == null)
                        {
                            storageRentalPaymentType = storageRentalPaymentTypes.FirstOrDefault(x => x.StorageRentalPaymentTypeName == "Well");
                            string[] subs = paymentTypeFromAccess.Split(' ');
                            if (subs != null && subs.Length > 2)
                            {
                                well = subs[2].Trim();
                            }
                        }


                        storageRentals.Add(new StorageRental
                        {
                            FkStorageId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("FKStorageId")),
                            RentPay = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RentPay")) ? (double?)null : (double?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("RentPay")),
                            StorageRentalEntryDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EntryDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EntryDate")),
                            StorageRentalNotes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                            PaymentPeriodYear = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PaymentPeriodYear")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("PaymentPeriodYear")),
                            CheckId = check.Id,
                            StorageRentalPaymentTypeId = storageRentalPaymentType.Id,
                            Well = well,
                            PeriodTypeId = periodTypeId
                        });
                    }
                }
            }

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT  * from dbo_tblStorage", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    long? fkLesseeId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkLesseeId")) ? (long?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeId"));
                    string contractNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ContractNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("ContractNum"));
                    string AltIdInformation = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("FieldName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("FieldName"));
                    string leaseNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LeaseNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("LeaseNum"));

                    StorageMonthLookup storageMonthLookup = storageMonthLookups.FirstOrDefault(x => x.LeaseNum == leaseNum);
                    if (storageMonthLookup == null)
                    {
                        storageMonthLookup = storageMonthLookups.FirstOrDefault(x => x.LeaseNum == null);
                    }

                    List<StorageBaseRentalPaymentMonthJunction> storageBaseRentalPaymentMonthJunctions = new List<StorageBaseRentalPaymentMonthJunction>();
                    List<StorageRentalPaymentMonthJunction> storageRentalPaymentMonthJunctions = new List<StorageRentalPaymentMonthJunction>();
                    List<StorageWellPaymentMonthJunction> storageWellPaymentMonthJunctions = new List<StorageWellPaymentMonthJunction>();
                    if (storageMonthLookup.BaseRentalMonths != null)
                    {
                        foreach (int monthNum in storageMonthLookup.BaseRentalMonths)
                        {
                            Month month = months.First(x => x.MonthNum == monthNum);
                            storageBaseRentalPaymentMonthJunctions.Add(new StorageBaseRentalPaymentMonthJunction { MonthId = month.Id });
                        }
                    }
                    if (storageMonthLookup.RentalMonths != null)
                    {
                        foreach (int monthNum in storageMonthLookup.RentalMonths)
                        {
                            Month month = months.First(x => x.MonthNum == monthNum);
                            storageRentalPaymentMonthJunctions.Add(new StorageRentalPaymentMonthJunction { MonthId = month.Id });
                        }
                    }
                    if (storageMonthLookup.WellMonths != null)
                    {
                        foreach (int monthNum in storageMonthLookup.WellMonths)
                        {
                            Month month = months.First(x => x.MonthNum == monthNum);
                            storageWellPaymentMonthJunctions.Add(new StorageWellPaymentMonthJunction { MonthId = month.Id });
                        }
                    }

                    var landLeaseAgreement = landLeaseAgreements.Where(x => x.AltIdInformation == AltIdInformation).FirstOrDefault();
                    if (landLeaseAgreement != null)
                    {
                        List<StorageRental> filteredStorageRentals = storageRentals.Where(x => x.FkStorageId == DB_Reader.GetInt32(DB_Reader.GetOrdinal("PKStorageId"))).ToList();

                        retval.Add(new Storage
                        {
                            Id = 0,
                            PKStorageId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("PKStorageId")),
                            ContractId = landLeaseAgreement.ContractId,
                            StorageNotes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                            StorageBaseRentalPayment = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("BaseRentalAmount")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("BaseRentalAmount")),
                            StorageWellPayment = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("WellRoyaltyAmount")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("WellRoyaltyAmount")),
                            NumOfWells = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("NumOfWells")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("NumOfWells")),
                            StorageNotesAdditional = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("NotesAdditional")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("NotesAdditional"))),
                            Terminated = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Terminated")) ? false : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Terminated")),
                            LeasedAcreage = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LeasedAcreage")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("LeasedAcreage")),
                            InPoolLeasedAcreage = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("InPoolLeasedAcreage")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("InPoolLeasedAcreage")),
                            InPoolFieldAcreage = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("InPoolFieldAcreage")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("InPoolFieldAcreage")),
                            COPFieldShare = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("COPFieldShare")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("COPFieldShare")),
                            LeaseNum = leaseNum,
                            StorageRentals = filteredStorageRentals,
                            SubjectToInflationInd = false,
                            StorageBaseRentalPaymentMonthJunctions = storageBaseRentalPaymentMonthJunctions,
                            StorageRentalPaymentMonthJunctions = storageRentalPaymentMonthJunctions,
                            StorageWellPaymentMonthJunctions = storageWellPaymentMonthJunctions,
                        });
                    }
                }
            }
            return retval;
        }

        public List<Storage> insertStorages(List<Storage> storages)
        {
            using var ctx = new DataContext();
            ctx.Storages.AddRange(storages);
            ctx.SaveChanges();
            return storages;
        }

        public List<Royalty> GetRoyalties(List<RevenueReceived> insertedRevuesReceived, List<PaymentType> paymentTypes, List<ProductType> productTypes)
        {
            List<Royalty> retval = new List<Royalty>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT  * from dbo_tblRoyalty", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                PaymentType paymentType = paymentTypes.First(x => x.PaymentTypeName == "Oil and Gas Royalty");

                while (DB_Reader.Read())
                {
                    RevenueReceived revenueReceived = new RevenueReceived();
                    long? fkCheckId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkCheckId")) ? (long?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkCheckId"));

                    if (fkCheckId != null)
                    {
                        revenueReceived = insertedRevuesReceived.FirstOrDefault(x => x.PKCheckId == fkCheckId);
                    }

                    if (revenueReceived?.Id != null && revenueReceived.Id > 0)
                    {
                        retval.Add(new Royalty
                        {
                            Id = 0,
                            FKWellOpsId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("FKWellOpsId")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("FKWellOpsId")),
                            PKRoyaltyId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PKRoyaltyId")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("PKRoyaltyId")),
                            FKCheckId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("FKCheckId")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("FKCheckId")),
                            RevenueReceivedId = revenueReceived.Id,
                            PostMonth = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PostMonth")) ? (int?)null : DB_Reader.GetByte(DB_Reader.GetOrdinal("PostMonth")),
                            PostYear = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PostYear")) ? (int?)null : DB_Reader.GetInt16(DB_Reader.GetOrdinal("PostYear")),
                            GasRoyalty = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("GasRoyalty")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("GasRoyalty")),
                            OilRoyalty = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("OilRoyalty")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("OilRoyalty")),
                            GasProd = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("GasProd")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("GasProd")),
                            OilProd = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("OilProd")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("OilProd")),
                            Deduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Deduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("Deduction")),
                            TransDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("TransDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("TransDeduction")),
                            CompressDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CompressDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("CompressDeduction")),
                            Flaring = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Flaring")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Flaring")),
                            RecvDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RecvDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("RecvDate")),
                            EntryDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EntryDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EntryDate")),
                            TransmittalDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("TransmittalDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("TransmittalDate")),
                            RoyaltyNotes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                            FirstTimePayment = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("FirstTimePayment")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("FirstTimePayment")),
                            SalesPrice = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("SalesPrice")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("SalesPrice")),
                            TrOpsShare = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("TrOpsShare")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("TrOpsShare")),
                            RentPayment = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RentPayment")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("RentPayment")),
                            StoragePayment = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("StoragePayment")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("StoragePayment")),
                            OtherPayment = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("OtherPayment")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("OtherPayment")),
                            NRI = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("NRI")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("NRI")),
                            UAFuelDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("UAFuelDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("UAFuelDeduction")),
                            QGatheringDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("2QGatheringDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("2QGatheringDeduction")),
                            QCompressionDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("5QCompressionDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("5QCompressionDeduction")),
                            MarketingDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("6CMarketingDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("6CMarketingDeduction")),
                            EightTTransDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("8TTransDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("8TTransDeduction")),
                            CompDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("9CCompDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("9CCompDeduction")),
                            FeulDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("9FFuelDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("9FFuelDeduction")),
                            GGatheringDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("9GGatheringDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("9GGatheringDeduction")),
                            NineTTransDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("9TTransDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("9TTransDeduction")),
                            AOMiscDeduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("AOMiscDeduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("AOMiscDeduction")),
                            CheckNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum")),
                            CheckDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("CheckDate")),
                            PaymentTypeId = paymentType.Id
                        });
                    }
                }
            }
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT  * from tblLiquidsRoyalty", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                PaymentType paymentType = paymentTypes.First(x => x.PaymentTypeName == "Liquids Royalty");

                while (DB_Reader.Read())
                {
                    long? productTypeId = null;
                    string productTypeName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Product")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Product"));
                    ProductType productType = productTypes.Where(x => !string.IsNullOrEmpty(productTypeName) && x.ProductTypeName == productTypeName).FirstOrDefault();
                    if (productType != null)
                    {
                        productTypeId = productType.Id;
                    }
                    retval.Add(new Royalty
                    {
                        Id = 0,
                        FKWellOpsId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("FKWellOpsId")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("FKWellOpsId")),
                        FKCheckId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("FKCheckId")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("FKCheckId")),
                        PKLiqRoyId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PKLiqRoyId")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("PKLiqRoyId")),
                        PostMonth = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Posting Month")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("Posting Month")),
                        PostYear = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Posting Year")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("Posting Year")),
                        CheckNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum")),
                        EntryDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EntryDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EntryDate")),
                        RecvDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EntryDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EntryDate")),
                        NRI = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("NRI")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("NRI")),
                        Deduction = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Deduction")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("Deduction")),
                        RoyaltyNotes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                        PaymentTypeId = paymentType.Id,
                        LiqVolume = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LiqVolume")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("LiqVolume")),
                        LiqPayment = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LiqPayment")) ? (decimal?)null : (decimal?)DB_Reader.GetDouble(DB_Reader.GetOrdinal("LiqPayment")),
                        LiqMeasurement = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LiqMeasurement")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("LiqMeasurement")),
                        ProductTypeId = productTypeId
                    });
                }

            }
            return retval;
        }

        public List<Royalty> InsertRoyalties(List<Royalty> royalties)
        {
            using var ctx = new DataContext();
            ctx.BulkInsert(royalties, new BulkConfig { PreserveInsertOrder = true });
            var insertedRoyalties = ctx.Royalties.OrderBy(x => x.Id).ToList();
            for (int i = 0; i < insertedRoyalties.Count; i++)
            {
                royalties[i].Id = insertedRoyalties[i].Id;
            }
            return royalties;
        }

        public List<RoyaltyAdjustment> InsertRoyaltyAdjustments(List<RoyaltyAdjustment> royaltyAdjustments)
        {
            using var ctx = new DataContext();
            ctx.BulkInsert(royaltyAdjustments, new BulkConfig { PreserveInsertOrder = true });
            var insertedRoyaltyAdjustments = ctx.RoyaltyAdjustments.OrderBy(x => x.Id).ToList();
            for (int i = 0; i < insertedRoyaltyAdjustments.Count; i++)
            {
                royaltyAdjustments[i].Id = insertedRoyaltyAdjustments[i].Id;
            }
            return royaltyAdjustments;
        }


        public List<TblTractOptShare> GetTblTractOptShares()
        {
            List<TblTractOptShare> retval = new List<TblTractOptShare>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from tblTractOpShare", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    retval.Add(new TblTractOptShare
                    {
                        PKTractId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("pkTractID")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("pkTractID")),
                        LesseeName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LesseeName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("LesseeName")),
                        StartDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("StartDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("StartDate")),
                        EndDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EndDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EndDate")),
                        TrOpShare = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("TrOpShare")) ? (Decimal?)null : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("TrOpShare")),
                        Horizon = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Horizon")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Horizon")),
                        Operator = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Operator")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Operator")),
                        Reason = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Reason")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Reason")),
                        Acres = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Acres")) ? (Decimal?)null : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("Acres")),
                    });
                }
            }

            return retval;
        }

        public List<WellOperation> GetWellOperations(List<Well> insertedWells, List<Lessee> insertedLeeses)
        {
            List<WellOperation> retval = new List<WellOperation>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblkWellOps", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    long? fkWellId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkWellId")) ? (long?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkWellId"));
                    if (fkWellId != null && fkWellId > 0)
                    {
                        Well well = insertedWells.FirstOrDefault(x => x.PKWellId == fkWellId);
                        long? fkLesseeId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkLesseeId")) ? (long?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeId"));
                        if (fkLesseeId != null && fkLesseeId > 0)
                        {
                            Lessee lessee = insertedLeeses.FirstOrDefault(x => x.PKLesseeId == fkLesseeId);

                            if (lessee != null && well != null && lessee?.Id != null && lessee.Id > 0 && well?.Id != null && well.Id > 0)

                            /* if (!DB_Reader.IsDBNull(DB_Reader.GetOrdinal("OpShare")))
                             {
                                 var i = DB_Reader.GetOrdinal("OpShare");
                                 var dotNetType = DB_Reader.GetFieldType(i).ToString();
                                 var sqlType = DB_Reader.GetDataTypeName(i);
                                 var specificType = DB_Reader.GetProviderSpecificFieldType(i);

                                 var z = "y";
                             }*/


                            {
                                retval.Add(new WellOperation
                                {
                                    Id = 0,
                                    PKWellOpsId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkWellOpsId")),
                                    LesseeId = lessee.Id,
                                    WellId = well.Id,
                                    OpShare = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("OpShare")) ? (Single?)null : DB_Reader.GetFloat(DB_Reader.GetOrdinal("OpShare")),
                                    ReportsTotal = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ReportsTotal")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("ReportsTotal")),
                                    Notes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes")))
                                });
                            }
                        }
                    }
                }
            }
            return retval;
        }

        public List<SurfaceUseAgreement> getSurfaceUseAgreements(List<ContractType> contractTypes,
            List<Tract> tracts, List<Lessee> lessees)
        {
            List<SurfaceUseAgreement> retval = new List<SurfaceUseAgreement>();
            long contractTypeId = contractTypes.FirstOrDefault(x => x.ContractTypeName == "SUA").Id;
            List<ContractSubType> contractSubTypes = contractTypes.Where(x => x.ContractTypeName == "SUA").SelectMany(c => c.ContractSubTypes).ToList();

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from tblSUA", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    string contractNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ContractNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("ContractNum"));
                    string fkTractID = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkTractID")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("fkTractID"));
                    var tractId = tracts.FirstOrDefault(x => x.PKTractId == fkTractID).Id;
                    long? fkLesseeId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkLesseeId")) ? (long?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeId"));
                    var lesseeId = lessees.FirstOrDefault(x => x.PKLesseeId == fkLesseeId).Id;
                    retval.Add(new SurfaceUseAgreement
                    {
                        Id = 0,
                        LesseeId = lesseeId,
                        CoalSUA = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Coal SUA")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Coal SUA")),
                        OandGSUA = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("O&G SUA")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("O&G SUA")),
                        EffectiveDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EffectiveDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EffectiveDate")),
                        EntryDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EntryDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EntryDate")),
                        TerminationDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("TerminatedDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("TerminatedDate")),
                        Notes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                        Terminated = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Terminated")) ? (Boolean?)null : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Terminated")),
                        Contract = new Contract
                        {
                            Id = 0,
                            ContractNum = contractNum,
                            ContractTypeId = contractTypeId,
                            ContractSubTypeId = GetContractSubTypeId(contractNum, contractSubTypes, tracts.FirstOrDefault(x => x.PKTractId == fkTractID)?.LandIncluded),
                            PaymentRequirement = new PaymentRequirement
                            {
                                Id = 0,

                            }
                        }
                    });
                }
            }
            return retval;
        }

        public List<WellOperation> InsertWellOperations(List<WellOperation> wellOperations)
        {
            //get rid of any duplicate lesseeid / well id rows
            List<WellOperation> groupedWellOperations = (from element in wellOperations
                                                         group element by new { element.LesseeId, element.WellId }
                                                        into groups
                                                         select groups.OrderBy(p => p.Id).First()).ToList();

            using var ctx = new DataContext();
            ctx.WellOperations.AddRange(groupedWellOperations);
            ctx.SaveChanges();
            return groupedWellOperations;
        }

        public List<Royalty> UpdateRoyalties(List<Royalty> insertedroyalties, List<WellTractInformation> insertedWellTractInformations)
        {
            using (var ctx = new DataContext())

            {
                foreach (Royalty royalty in insertedroyalties)
                {
                    var wellTractInformation = insertedWellTractInformations.FirstOrDefault(x => x.PKWellOpsId == royalty.FKWellOpsId);
                    if (wellTractInformation != null)
                    {
                        royalty.WellTractInformationId = wellTractInformation.Id;
                    }
                }
                ctx.Database.SetCommandTimeout(400);
                ctx.BulkUpdate(insertedroyalties);

            }
            return insertedroyalties;
        }

        public List<Role> InsertRoles()
        {
            List<Role> roles = new List<Role>()
           {
               new Role()
               {
                 RoleName = "read",
                 Description = "may read data but not update delete or insert"
               },
               new Role()
               {
                    RoleName = "write",
                    Description = "may read, update, delete, or insert data"
               },
                 new Role()
               {
                    RoleName = "admin",
                    Description = "administrator"
               }
           };
            using var ctx = new DataContext();
            ctx.Roles.AddRange(roles);
            ctx.SaveChanges();
            return roles;
        }

        public List<ContractEventDetailReasonForChange> InsertcontractEventDetailReasonsForChange()
        {
            List<ContractEventDetailReasonForChange> contractEventDetailReasonsForChange = new List<ContractEventDetailReasonForChange>()
            {
                new ContractEventDetailReasonForChange()
                {
                    Reason = "Bankruptcy"
                },
                 new ContractEventDetailReasonForChange()
                {
                    Reason = "C2A"
                },
                  new ContractEventDetailReasonForChange()
                {
                    Reason = "Contract Start"
                },

                     new ContractEventDetailReasonForChange()
                {
                    Reason = "Correction / Typo"
                },
                  new ContractEventDetailReasonForChange()
                {
                    Reason = "Farm Out"
                },
                   new ContractEventDetailReasonForChange()
                {
                    Reason = "Lease Amendment"
                },
                  new ContractEventDetailReasonForChange()
                {
                    Reason = "Lease Surrender"
                },
                 new ContractEventDetailReasonForChange()
                {
                    Reason = "Name Change"
                },
                    new ContractEventDetailReasonForChange()
                {
                    Reason = "Replacement Agreement"
                },
                 new ContractEventDetailReasonForChange()
                {
                    Reason = "Surrender Acreage"
                },
                new ContractEventDetailReasonForChange()
                {
                    Reason = "Transfer of Ownership"
                },
            };
            using var ctx = new DataContext();
            ctx.ContractEventDetailReasonsForChange.AddRange(contractEventDetailReasonsForChange);
            ctx.SaveChanges();
            return contractEventDetailReasonsForChange.ToList();
        }

        public List<ContractType> InsertContractTypes()
        {
            List<ContractType> ContractTypes = new List<ContractType>()
            {
                new ContractType()
                {
                    ContractTypeName = "SUA",
                    MapType = "X",
                    ContractSubTypes = new List<ContractSubType>(){
                    new ContractSubType()
                    {
                        ContractSubTypeName = "Oil and Gas",
                        ContractNumPrefix = "M-600"
                    },
                    new ContractSubType()
                    {
                        ContractSubTypeName = "Coal",
                        ContractNumPrefix = "M-620"
                    }
                  }
                },
                new ContractType()
                {
                    ContractTypeName = "Land Lease",
                     MapType = "X",
                     ContractSubTypes = new List<ContractSubType>(){
                        new ContractSubType()
                    {
                        ContractSubTypeName = "Streambed",
                        ContractNumPrefix = "M-210"
                    },
                       new ContractSubType()
                    {
                        ContractSubTypeName = "DGS Land",
                        ContractNumPrefix = "M-310"
                    },
                        new ContractSubType()
                    {
                        ContractSubTypeName = "State Forest",
                        ContractNumPrefix = "M-110"
                    },
                         new ContractSubType()
                    {
                        ContractSubTypeName = "State Park",
                        ContractNumPrefix = "M-110"
                    },
                        new ContractSubType()
                    {
                        ContractSubTypeName = "State Forest & State Park",
                        ContractNumPrefix = "M-110"
                    },
                         new ContractSubType()
                    {
                        ContractSubTypeName = "Gas Storage Field",
                         ContractNumPrefix = "M-120",
                         MapTypeOverride = "Z"
                    },
                         new ContractSubType()
                    {
                        ContractSubTypeName = "NSD Oil & Gas Lease",
                         ContractNumPrefix = "M-410"
                    },
                  }
                },
                new ContractType()
                {
                    ContractTypeName = "Prospecting Agreement",
                     MapType = "Z",
                    ContractNumPrefix = "M-400"
                },
                   new ContractType()
                {
                    ContractTypeName = "Production Agreement",
                     MapType = "X",
                    ContractNumPrefix = "M-500"
                },
                   new ContractType()
                {
                    ContractTypeName = "Seismic Agreement",
                     MapType = "Z",
                    ContractNumPrefix = "M-"
                },

            };
            using var ctx = new DataContext();
            ctx.ContractTypes.AddRange(ContractTypes);
            ctx.SaveChanges();
            return ContractTypes.ToList();
        }

        public List<SurfaceUseAgreement> insertSurfaceUseAgreements(List<SurfaceUseAgreement> surfaceUseAgreements, List<Tract> tracts, List<ContractType> contractTypes)
        {
            long contractTypeId = contractTypes.FirstOrDefault(x => x.ContractTypeName == "SUA").Id;
            List<ContractSubType> contractSubTypes = contractTypes.Where(x => x.ContractTypeName == "SUA").SelectMany(c => c.ContractSubTypes).ToList();
            var suaTracts = tracts.Where(x => x.ContractNum != null && x.ContractNum.StartsWith("M-6"));
            using var ctx = new DataContext();
            var lessees = ctx.Lessees;
            foreach (var item in suaTracts)
            {

                SurfaceUseAgreement su = new SurfaceUseAgreement
                {
                    Id = 0,
                    LesseeId = null,
                    Contract = new Contract
                    {
                        Id = 0,
                        ContractNum = item.ContractNum,
                        ContractTypeId = contractTypeId,
                        ContractSubTypeId = GetContractSubTypeId(item.ContractNum, contractSubTypes, item.LandIncluded),
                        TractId = item.Id,
                        PaymentRequirement = new PaymentRequirement
                        {
                            Id = 0,
                        }
                    }
                };
                surfaceUseAgreements.Add(su);
            }
            ctx.SurfaceUseAgreements.AddRange(surfaceUseAgreements);
            ctx.SaveChanges();
            return surfaceUseAgreements.ToList();
        }

        public List<LandLeaseAgreement> insertLandLeaseAgreements(List<Tract> tracts, List<ContractType> contractTypes)
        {
            long contractTypeId = contractTypes.FirstOrDefault(x => x.ContractTypeName == "Land Lease").Id;
            List<ContractSubType> contractSubTypes = contractTypes.Where(x => x.ContractTypeName == "Land Lease").SelectMany(c => c.ContractSubTypes).ToList();
            var landLeaseTracts = tracts.Where(x => x.ContractNum != null && (x.ContractNum.StartsWith("M-1") || x.ContractNum.StartsWith("M-2") || x.ContractNum.StartsWith("M-3")));
            List<LandLeaseAgreement> landLeaseAgreements = new List<LandLeaseAgreement>();
            using var ctx = new DataContext();
            List<Month> months = ctx.Months.ToList();
            foreach (var item in landLeaseTracts)
            {
                List<ContractRentalPaymentMonthJunction> contractRentalPaymentMonthJunctions = new List<ContractRentalPaymentMonthJunction>();
                if (item.RentalMonthDue != null)
                {
                    Month month = months.Where(x => x.MonthNum == item.RentalMonthDue.Value.Month).First();
                    contractRentalPaymentMonthJunctions.Add(new ContractRentalPaymentMonthJunction
                    {
                        MonthId = month.Id
                    });
                }
                else
                {
                    if (item.ContractNum == "M-110770")
                    {
                        int[] quarterMonths = new int[] { 3, 6, 9, 12 };
                        foreach (var quarterMonth in months.Where(x => quarterMonths.Contains(x.MonthNum)))
                        {
                            contractRentalPaymentMonthJunctions.Add(new ContractRentalPaymentMonthJunction
                            {
                                MonthId = quarterMonth.Id
                            });
                        }
                    }
                }
                LandLeaseAgreement la = new LandLeaseAgreement
                {
                    Id = 0,
                    EffectiveDate = item.LeaseDate,
                    TerminationDate = item.TerminatedDate,
                    Acreage = item.Acreage,
                    Contract = new Contract
                    {
                        Id = 0,
                        ContractNum = item.ContractNum,
                        ContractTypeId = contractTypeId,
                        ContractSubTypeId = GetContractSubTypeId(item.ContractNum, contractSubTypes, item.LandIncluded),
                        TractId = item.Id,
                        ContractRentalPaymentMonthJunctions = contractRentalPaymentMonthJunctions,
                        PaymentRequirement = new PaymentRequirement
                        {
                            Id = 0,
                        }
                    }
                };
                landLeaseAgreements.Add(la);
            }

            ctx.LandLeaseAgreements.AddRange(landLeaseAgreements);
            ctx.SaveChanges();
            return landLeaseAgreements.ToList();
        }

        public List<ProspectingAgreement> insertProspectingAgreements(List<Tract> tracts, List<ContractType> contractTypes)
        {
            long contractTypeId = contractTypes.FirstOrDefault(x => x.ContractTypeName == "Prospecting Agreement").Id;
            var prospectingracts = tracts.Where(x => x.ContractNum != null && x.ContractNum.StartsWith("M-4"));
            List<ProspectingAgreement> prospectingAgreements = new List<ProspectingAgreement>();
            foreach (var item in prospectingracts)
            {
                ProspectingAgreement pa = new ProspectingAgreement
                {
                    Id = 0,
                    Contract = new Contract
                    {
                        Id = 0,
                        ContractNum = item.ContractNum,
                        ContractTypeId = contractTypeId,
                        ContractSubTypeId = null,
                        TractId = item.Id,
                        PaymentRequirement = new PaymentRequirement
                        {
                            Id = 0,
                        }
                    }
                };
                prospectingAgreements.Add(pa);
            }
            using var ctx = new DataContext();
            ctx.ProspectingAgreements.AddRange(prospectingAgreements);
            ctx.SaveChanges();
            return prospectingAgreements.ToList();
        }

        public List<Formation> GetFormations()
        {
            List<Formation> retval = new List<Formation>();

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from tblFormations", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    retval.Add(new Formation
                    {
                        Id = 0,
                        FormationName = DB_Reader.GetString(DB_Reader.GetOrdinal("FormationName"))
                    });
                }
                retval.Add(new Formation
                {
                    Id = 0,
                    FormationName = "Unknown"
                });

                return retval;
            }
        }

        public List<District> GetDistricts()
        {
            List<District> retval = new List<District>();

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tbluDistrict", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    retval.Add(new District
                    {
                        Id = 0,
                        DistrictId = DB_Reader.GetByte(DB_Reader.GetOrdinal("pkDistrictID")),
                        Name = DB_Reader.GetString(DB_Reader.GetOrdinal("DistName"))
                    });
                }

                return retval;
            }
        }

        public List<District> InsertDistricts(List<District> districts)
        {
            using var ctx = new DataContext();
            ctx.Districts.AddRange(districts);
            ctx.SaveChanges();
            return districts.ToList();
        }

        public List<Formation> InsertFormations(List<Formation> formations)
        {
            using var ctx = new DataContext();
            ctx.Formations.AddRange(formations);
            ctx.SaveChanges();
            return formations.ToList();
        }


        public List<DistrictContractJunction> insertDistrictContractJunctions()
        {
            List<DistrictContractJunction> retval = new List<DistrictContractJunction>();
            using var ctx = new DataContext();
            List<Contract> contracts = ctx.Contracts.ToList();
            List<District> districts = ctx.Districts.ToList();
            foreach (var contract in contracts)
            {
                if (contract.ContractNum != null && contract.ContractNum.Contains("-") && contract.ContractNum.Count(f => f == '-') > 1)
                {

                    int index = contract.ContractNum.IndexOf('-', contract.ContractNum.IndexOf('-') + 1);
                    string temp = contract.ContractNum.Substring(index + 1);
                    string temp2 = temp.Replace('-', '&');
                    foreach (var item in temp2.Split('&'))
                    {
                        string districtId = Regex.Replace(item, "[^0-9.]", "");
                        bool success = Int64.TryParse(districtId, out long number);
                        if (success)
                        {
                            if (districts.FirstOrDefault(x => x.DistrictId == number)?.DistrictId != null)
                            {
                                retval.Add(new DistrictContractJunction
                                {
                                    Id = 0,
                                    ContractId = contract.Id,
                                    DistrictId = districts.First(x => x.DistrictId == number).Id
                                });
                            }

                        }
                    }

                }

            }
            ctx.DistrictContractJunctions.AddRange(retval);
            ctx.SaveChanges();
            return retval;
        }

        public List<State> GetStates()
        {
            List<State> states = new List<State>();
            using (OleDbConnection cn = new OleDbConnection(townshipConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    {
                        cmd.CommandText = "SELECT * FROM [states.csv]";
                        cmd.CommandType = CommandType.Text;
                        using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            int fieldName = reader.GetOrdinal("Name");
                            int fieldCode = reader.GetOrdinal("Code");
                            foreach (DbDataRecord record in reader)
                            {
                                states.Add(new State
                                {
                                    Id = 0,
                                    Name = record.GetString(fieldName),
                                    Code = record.GetString(fieldCode)
                                });
                            }
                        }
                    }
                }
            }
            return states;
        }

        public List<ApiCode> GetApiCodes()
        {
            List<ApiCode> apiCodes = new List<ApiCode>();
            using (OleDbConnection cn = new OleDbConnection(townshipConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [ApiCodes.csv]";
                    cmd.CommandType = CommandType.Text;
                    using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        //int fieldStateCode = reader.GetOrdinal("StateCode");
                        // int fieldStateName = reader.GetOrdinal("StateName");
                        // int fieldCountyCode = reader.GetOrdinal("CountyCode");
                        // int fieldCountyName = reader.GetOrdinal("CountyName");
                        foreach (DbDataRecord record in reader)
                        {
                            if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2) && !reader.IsDBNull(3))
                            {
                                apiCodes.Add(new ApiCode
                                {
                                    Id = 0,
                                    StateCode = reader.GetInt32(0),
                                    StateName = reader.GetString(1),
                                    CountyCode = reader.GetInt32(2),
                                    CountyName = reader.GetString(3)
                                });
                            }
                        }
                    }
                }
            }
            return apiCodes;
        }

        public List<AltContractExcel> GetAltContractExcelFile()
        {
            List<AltContractExcel> files = new List<AltContractExcel>();
            using (OleDbConnection cn = new OleDbConnection(townshipConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    {
                        cmd.CommandText = "SELECT * FROM [AltWellIds.csv]";
                        cmd.CommandType = CommandType.Text;
                        using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            foreach (DbDataRecord record in reader)
                            {
                                files.Add(new AltContractExcel
                                {
                                    ApiNum = record.GetString(0),
                                    AltId = record.GetString(1)
                                });
                            }
                        }
                    }
                }
            }

            return files;
        }

        public List<LesseeNameChangeExcelFile> GetLesseeNameChangeExcelFile()
        {
            List<LesseeNameChangeExcelFile> files = new List<LesseeNameChangeExcelFile>();
            using (OleDbConnection cn = new OleDbConnection(townshipConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    {
                        cmd.CommandText = "SELECT * FROM [LesseeHistorical.csv]";
                        cmd.CommandType = CommandType.Text;
                        using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            foreach (DbDataRecord record in reader)
                            {
                                files.Add(new LesseeNameChangeExcelFile
                                {
                                    CurrentLesseeName = record.GetString(0),
                                    HistoricalLesseeName = record.GetString(1)
                                });
                            }
                        }
                    }
                }
            }

            return files;
        }

        public List<NonUnitizedWellsAcre> GetNonUnitizedWellAcres()
        {
            List<NonUnitizedWellsAcre> nonUnitizedWellsAcres = new List<NonUnitizedWellsAcre>();
            using (OleDbConnection cn = new OleDbConnection(townshipConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    {
                        cmd.CommandText = "SELECT * FROM [nonUnitizedWells.csv]";
                        cmd.CommandType = CommandType.Text;
                        using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            foreach (DbDataRecord record in reader)
                            {
                                nonUnitizedWellsAcres.Add(new NonUnitizedWellsAcre
                                {
                                    ApiNum = record.GetString(0),
                                    AcreageAttributableToWells = (double)record.GetInt32(1)
                                });
                            }
                        }
                    }
                }
            }
            return nonUnitizedWellsAcres;
        }

        public List<WellOperator> GetWellOperators()
        {
            List<WellOperator> wellOperators = new List<WellOperator>();
            using (OleDbConnection cn = new OleDbConnection(townshipConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    {
                        cmd.CommandText = "SELECT * FROM [WellOperators.csv]";
                        cmd.CommandType = CommandType.Text;
                        using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            foreach (DbDataRecord record in reader)
                            {
                                wellOperators.Add(new WellOperator
                                {
                                    Operator = record.GetString(0),
                                    ApiNum = record.GetString(1)
                                });
                            }
                        }
                    }
                }
            }

            return wellOperators;
        }

        public List<WellOpsAltIdLookup> GetWellOpsAltIdLookups()
        {
            List<WellOpsAltIdLookup> wellOpsAltIdLookups = new List<WellOpsAltIdLookup>();
            using (OleDbConnection cn = new OleDbConnection(townshipConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    {
                        cmd.CommandText = "SELECT * FROM [tblAnadarkoWellCodes.csv]";
                        cmd.CommandType = CommandType.Text;
                        using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            foreach (DbDataRecord record in reader)
                            {
                                if (!record.IsDBNull(6))
                                {
                                    wellOpsAltIdLookups.Add(new WellOpsAltIdLookup
                                    {
                                        PKWellOpsId = record.GetInt32(4),
                                        PropertyNumber = record.GetInt32(6)
                                    });
                                }
                          
                            }
                        }
                    }
                }
            }

            return wellOpsAltIdLookups;

        }

        public List<Township> GetTownShips()
        {
            List<Township> townships = new List<Township>();
            using (OleDbConnection cn = new OleDbConnection(townshipConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    {
                        cmd.CommandText = "SELECT * FROM [municipalities.csv]";
                        cmd.CommandType = CommandType.Text;
                        using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            int fieldCounty = reader.GetOrdinal("COUNTY");
                            int fieldMunicipality = reader.GetOrdinal("MUNICIPALITY");
                            int fieldClass = reader.GetOrdinal("CLASS");

                            foreach (DbDataRecord record in reader)
                            {
                                townships.Add(new Township
                                {
                                    Id = 0,
                                    County = record.GetString(fieldCounty),
                                    Municipality = record.GetString(fieldMunicipality),
                                    Class = record.GetString(fieldClass),
                                });
                            }
                        }
                    }
                }
            }
            return townships;
        }

        public List<Township> InsertTownships(List<Township> townships)
        {
            using var ctx = new DataContext();
            ctx.Townships.AddRange(townships);
            ctx.SaveChanges();
            return townships.ToList();

        }

        public List<State> InsertStates(List<State> states)
        {
            using var ctx = new DataContext();
            ctx.States.AddRange(states);
            ctx.SaveChanges();
            return states.ToList();
        }

        public List<ApiCode> InsertApiCodes(List<ApiCode> apiCodes)
        {
            using var ctx = new DataContext();
            ctx.ApiCodes.AddRange(apiCodes);
            ctx.SaveChanges();
            return apiCodes.ToList();
        }

        public List<TerminationReason> InsertTerminationReasons()
        {
            List<TerminationReason> terminationReasons = new List<TerminationReason>() {
                new TerminationReason
                {
                    Id = 0,
                    Reason = "First well provision not met"
                },
                new TerminationReason
                {
                    Id = 0,
                    Reason = "Expiration of primary term with no royalties"
                },
                new TerminationReason
                {
                    Id = 0,
                    Reason = "Failure of well(s) to meet TWE"
                },
                new TerminationReason
                {
                    Id = 0,
                    Reason = "Operator terminated"
                },
                new TerminationReason
                {
                    Id = 0,
                    Reason = "Breach of lease"
                },
            };
            using var ctx = new DataContext();
            ctx.TerminationReasons.AddRange(terminationReasons);
            ctx.SaveChanges();
            return terminationReasons.ToList();
        }

        public List<AltIdCategory> InsertAltIdCategories()
        {
            List<AltIdCategory> altIdCategories = new List<AltIdCategory>() {
                new AltIdCategory
                {
                    Id = 0,
                    AltIdName = "Not Applicable"
                },
                new AltIdCategory
                {
                    Id = 0,
                    AltIdName = "External ID"
                },
                new AltIdCategory
                {
                    Id = 0,
                    AltIdName = "Internal ID"
                },
                   new AltIdCategory
                {
                    Id = 0,
                    AltIdName = "Storage Field Name"
                },
                  new AltIdCategory
                {
                    Id = 0,
                    AltIdName = "Streambed"
                },
                  new AltIdCategory
                {
                    Id = 0,
                    AltIdName = "Survey Name"
                },

            };
            using var ctx = new DataContext();
            ctx.AltIdCategories.AddRange(altIdCategories);
            ctx.SaveChanges();
            return altIdCategories.ToList();
        }

        public List<ContractEventDetail> InsertContractEventDetails(
            List<Tract> insertedTracts, List<TblTractOptShare> tblTractOptShares, List<TblTractLessee> tblTractLessees)
        {
            using var ctx = new DataContext();
            List<Contract> contracts = ctx.Contracts.ToList();
            List<ContractEventDetailReasonForChange> contractEventDetailReasonsForChange = ctx.ContractEventDetailReasonsForChange.ToList();
            List<Lessee> lessees = ctx.Lessees.ToList();
            List<ContractEventDetail> contractEventDetails = new List<ContractEventDetail>();
            foreach (var contract in contracts)
            {

                var tracts = insertedTracts.Where(x => x.ContractNum == contract.ContractNum).ToList();

                foreach (var tract in tracts)
                {

                    if (tract?.PKTractId != null)
                    {
                        List<TblTractOptShare> currentEvents = tblTractOptShares.Where(
                            x => (
                                    x.PKTractId == tract.PKTractId &&
                                    !(string.IsNullOrEmpty(x.LesseeName)) &&
                                    (string.IsNullOrEmpty(x.Reason) || string.IsNullOrWhiteSpace(x.Reason) || x.Reason == "NA")
                                )
                            ).ToList();

                        List<TblTractOptShare> historicalEvents = tblTractOptShares.Where(
                            x => x.PKTractId == tract.PKTractId &&
                            !(string.IsNullOrEmpty(x.LesseeName)) &&
                            !(string.IsNullOrEmpty(x.Reason)) &&
                            !(string.IsNullOrWhiteSpace(x.Reason)) &&
                            x.Reason != "NA").ToList();

                        if (currentEvents != null && currentEvents.Count > 0)
                        {

                            foreach (var currentEvent in currentEvents)
                            {
                                long? lesseeId = null;
                                var lessee = lessees.FirstOrDefault(x => x.LesseeName == currentEvent.LesseeName);
                                if (lessee?.Id != null)
                                {
                                    lesseeId = lessee.Id;
                                }


                                contractEventDetails.Add(new ContractEventDetail
                                {
                                    Id = 0,
                                    ActiveInd = true,
                                    LesseeName = currentEvent.LesseeName,
                                    EffectiveDate = currentEvent.StartDate,
                                    ShareOfLeasePercentage = currentEvent.TrOpShare,
                                    Acres = currentEvent.Acres,
                                    LesseeId = lesseeId,
                                    ContractId = contract.Id,
                                    Horizon = currentEvent.Horizon,
                                    IndustryLeaseInd = false,
                                    MinimumRoyalty = tract.RoyaltyFloor,
                                    RoyaltyPercent = tract.RoyaltyPercent,
                                });
                            }
                        }
                        if (historicalEvents != null && historicalEvents.Count > 0)
                        {

                            foreach (var historicalEvent in historicalEvents)
                            {
                                long? lesseeId = null;
                                var lessee = lessees.FirstOrDefault(x => x.LesseeName == historicalEvent.LesseeName);
                                if (lessee?.Id != null)
                                {
                                    lesseeId = lessee.Id;
                                }

                                long? contractEventDetailReasonForChangeId = null;
                                var contractEventDetailReasonForChange = contractEventDetailReasonsForChange.FirstOrDefault(x => x.Reason == historicalEvent.Reason);
                                if (contractEventDetailReasonForChange != null)
                                {
                                    contractEventDetailReasonForChangeId = contractEventDetailReasonForChange.Id;
                                }
                                contractEventDetails.Add(new ContractEventDetail
                                {
                                    Id = 0,
                                    ContractId = contract.Id,
                                    ActiveInd = false,
                                    LesseeName = historicalEvent.LesseeName,
                                    EffectiveDate = historicalEvent.StartDate,
                                    ShareOfLeasePercentage = historicalEvent.TrOpShare,
                                    Acres = historicalEvent.Acres,
                                    ContractEventDetailReasonForChangeId = contractEventDetailReasonForChangeId,
                                    LesseeId = lesseeId,
                                    Horizon = historicalEvent.Horizon,
                                    IndustryLeaseInd = false,
                                    MinimumRoyalty = tract.RoyaltyFloor,
                                    RoyaltyPercent = tract.RoyaltyPercent,
                                });
                            }
                        }
                    }
                }
            }

            // insert contract events for tracts without a tbltractoptshare
            var cedPKTractIds = from ced in contractEventDetails
                                join t in insertedTracts on ced.LesseeId equals t.LesseeId
                                select t.PKTractId
                                .Distinct();


            var allPKTractIds = insertedTracts.Select(x => x.PKTractId).Distinct();
            var test = allPKTractIds.Except(cedPKTractIds).ToArray();
            foreach (string item in allPKTractIds.Except(cedPKTractIds))
            {
                var t = insertedTracts.First(x => x.PKTractId == item);
                foreach (var ttl in tblTractLessees.Where(x => x.FKTractId == item))
                {
                    var l = lessees.Where(x => x.PKLesseeId == ttl.FKLesseeId).FirstOrDefault();
                    if (l != null)
                    {
                        var c = contracts.Where(x => x.ContractNum == t.ContractNum).FirstOrDefault();
                        if (c != null)
                        {
                            contractEventDetails.Add(new ContractEventDetail
                            {
                                Id = 0,
                                ActiveInd = true,
                                LesseeName = l.LesseeName,
                                EffectiveDate = t.LeaseDate,
                                ShareOfLeasePercentage = 0,
                                Acres = t.Acreage,
                                LesseeId = l.Id,
                                ContractId = c.Id,
                                Horizon = string.Empty,
                                IndustryLeaseInd = false,
                                MinimumRoyalty = t.RoyaltyFloor,
                                RoyaltyPercent = t.RoyaltyPercent,
                            });
                        }
                    }
                }

            }

            ctx.ContractEventDetails.AddRange(contractEventDetails);
            ctx.SaveChanges();
            return ctx.ContractEventDetails.ToList();
        }

        public void AddLesseeHistoricalRecords(List<LesseeNameChangeExcelFile> lesseeNameChangeExcelFiles, List<Lessee> insertedLeeses)
        {
            using var ctx = new DataContext();
            foreach (var item in lesseeNameChangeExcelFiles)
            {
                var lessee = insertedLeeses.Where(x => x.LesseeName == item.CurrentLesseeName).FirstOrDefault();
                var historicalLessee = insertedLeeses.Where(x => x.LesseeName == item.HistoricalLesseeName).FirstOrDefault();
                if (lessee != null && historicalLessee != null)
                {
                    foreach (var contractEventDetail in ctx.ContractEventDetails.Where(x => x.LesseeId == historicalLessee.Id))
                    {
                        contractEventDetail.LesseeId = lessee.Id;
                        if (contractEventDetail.ActiveInd)
                        {
                            contractEventDetail.LesseeName = lessee.LesseeName;
                        }
                    }
                    ctx.SaveChanges();
                    ctx.LesseeHistories.Add(new LesseeHistory
                    {
                        Id = 0,
                        LesseeId = lessee.Id,
                        LesseeName = historicalLessee.LesseeName
                    });
                    ctx.SaveChanges();
                    var hl = ctx.Lessees.FirstOrDefault(x => x.Id == historicalLessee.Id);
                    if (hl != null)
                    {
                        hl.LogicalDeleteIn = true;
                    }
                    ctx.SaveChanges();
                }

            }
        }

        public List<WellLogTestType> InsertWellLogTestTypes()
        {
            List<WellLogTestType> wellLogTestTypes = new List<WellLogTestType>()
            {
                new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "Caliper"
                },
                 new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "CBL"
                },
                   new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "Density"
                },
                  new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "Gamma Ray"
                },
                 new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "Lithology"
                },
                   new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "Microseismic"
                },
                     new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "Neutron"
                },
                   new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "Resistivity"
                },
                 new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "Sonic"
                },
                  new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "SP"
                },
                    new WellLogTestType
                {
                    Id = 0,
                    WellLogTestName = "VSP"
                },
            };
            using var ctx = new DataContext();
            ctx.WellLogTestTypes.AddRange(wellLogTestTypes);
            ctx.SaveChanges();
            return wellLogTestTypes.ToList();
        }

        public List<WellType> InsertWellTypes()
        {
            List<WellType> wellTypes = new List<WellType>()
            {
                new WellType
                {
                    Id = 0,
                    WellTypeName = "Gas"
                },
                new WellType
                {
                    Id = 0,
                    WellTypeName = "Oil"
                },
                new WellType
                {
                    Id = 0,
                    WellTypeName = "Condensate"
                },
                new WellType
                {
                    Id = 0,
                    WellTypeName = "Combination"
                },
                  new WellType
                {
                    Id = 0,
                    WellTypeName = "Storage"
                },
                   new WellType
                {
                    Id = 0,
                    WellTypeName = "Monitoring"
                },
            };
            using var ctx = new DataContext();
            ctx.WellTypes.AddRange(wellTypes);
            ctx.SaveChanges();
            return wellTypes.ToList();
        }

        public List<ProductType> InsertProductTypes()
        {
            List<ProductType> productTypes = new List<ProductType>
            {
                new ProductType{ProductTypeName = "Butane"},
                new ProductType{ProductTypeName = "Condensate"},
                new ProductType{ProductTypeName = "Ethane"},
                new ProductType{ProductTypeName = "NGL"},
                new ProductType{ProductTypeName = "Propane"},
            };
            using var ctx = new DataContext();
            ctx.ProductTypes.AddRange(productTypes);
            ctx.SaveChanges();
            return productTypes;
        }

        public List<PaymentType> InsertPaymentTypes()
        {
            List<PaymentType> paymentTypes = new List<PaymentType>
            {
                new PaymentType{PaymentTypeName = "Liquids Royalty"},
                new PaymentType{PaymentTypeName = "Oil and Gas Royalty"},
            };
            using var ctx = new DataContext();
            ctx.PaymentTypes.AddRange(paymentTypes);
            ctx.SaveChanges();
            return paymentTypes;
        }

        public List<WellStatus> InsertWellStatuses()
        {
            List<WellStatus> wellStatuses = new List<WellStatus>() {
                new WellStatus
                {
                    Id = 0,
                    WellStatusName = "O&A"
                },
                new WellStatus
                {
                    Id = 0,
                    WellStatusName = "Plugged"
                },
                new WellStatus
                {
                    Id = 0,
                    WellStatusName = "Producing"
                },
                 new WellStatus
                {
                    Id = 0,
                    WellStatusName = "Regulatory Inactive Status"
                },
                    new WellStatus
                {
                    Id = 0,
                    WellStatusName = "Shut-In"
                },
                     new WellStatus
                {
                    Id = 0,
                    WellStatusName = "Spud but not completed"
                },

            };
            using var ctx = new DataContext();
            ctx.WellStatuses.AddRange(wellStatuses);
            ctx.SaveChanges();
            return wellStatuses.ToList();
        }

        public List<WellStatusLookup> InsertWellStatusLookups()
        {
            return new List<WellStatusLookup>()
            {
                new WellStatusLookup{ WellStatusName = "O&A", AccessName = "Abandoned" },
                new WellStatusLookup{ WellStatusName = "O&A", AccessName = "Abanoned" },
                new WellStatusLookup{ WellStatusName = "O&A", AccessName = "CEP Orphan List" },
                new WellStatusLookup{ WellStatusName = "O&A", AccessName = "DEP Abandoned List" },
                new WellStatusLookup{ WellStatusName = "O&A", AccessName = "DEP Orpan List" },
                new WellStatusLookup{ WellStatusName = "O&A", AccessName = "DEP Orphan List" },
                new WellStatusLookup{ WellStatusName = "O&A", AccessName = "O&A" },
                new WellStatusLookup{ WellStatusName = "Plugged", AccessName = "Plugged" },
                new WellStatusLookup{ WellStatusName = "Producing", AccessName = "Producing" },
                new WellStatusLookup{ WellStatusName = "Regulatory Inactive Status", AccessName = "Regulatory Inactive Status" },
                new WellStatusLookup{ WellStatusName = "Severed Plugged", AccessName = "Plugged" },
                new WellStatusLookup{ WellStatusName = "Severed Producing", AccessName = "Producing" },
                new WellStatusLookup{ WellStatusName = "Severed Shut-In", AccessName = "Shut-In" },
                new WellStatusLookup{ WellStatusName = "Shut-In", AccessName = "Shut-In" },
                new WellStatusLookup{ WellStatusName = "Spud but not completed", AccessName = "Spud but not completed" },
            };

        }


        public List<TractUnitJunction> InsertTractUnitJunctions(List<Unit> insertedUnits, List<Tract> insertedTracts, List<Well> insertedWells)
        {
            using var ctx = new DataContext();
            List<TractUnitJunction> retval = new List<TractUnitJunction>();
            List<WellTractInformation> wellTractInformations = ctx.WellTractInformations.ToList();

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblkCopUnit", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {

                    long fkUnitId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkUnitId"));
                    string fkTractId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkTractId")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("fkTractId"));
                    var Tract = insertedTracts.FirstOrDefault(x => x.PKTractId == fkTractId);
                    var Unit = insertedUnits.FirstOrDefault(x => x.PKUnitId == fkUnitId);
                    if (Tract != null && Unit != null && !string.IsNullOrEmpty(fkTractId) && !string.IsNullOrWhiteSpace(fkTractId) && Unit.Id > 0 && Tract.Id > 0)
                    {
                        List<TractUnitJunctionWellJunction> tractUnitJunctionWellJunctions = new List<TractUnitJunctionWellJunction>();

                        foreach (var item in wellTractInformations.Where(x => x.TractId == Tract.Id))
                        {
                            var well = insertedWells.FirstOrDefault(x => x.Id == item.WellId);
                            if (well != null && well.FkUnitId != null && well.FkUnitId == fkUnitId)
                            {
                                tractUnitJunctionWellJunctions.Add(new TractUnitJunctionWellJunction
                                {
                                    Id = 0,
                                    WellId = item.WellId
                                });
                            }
                        }

                        retval.Add(new TractUnitJunction
                        {
                            Id = 0,
                            TractId = Tract.Id,
                            UnitId = Unit.Id,
                            COPAcres = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CopAcres")) ? (Double?)null : DB_Reader.GetDouble(DB_Reader.GetOrdinal("CopAcres")),
                            TractUnitJunctionWellJunctions = tractUnitJunctionWellJunctions
                        });
                    }

                }
            }


            ctx.TractUnitJunctions.AddRange(retval);
            ctx.SaveChanges();
            return retval.ToList();
        }

        public List<AppSetting> InsertAppSettings()
        {
            using var ctx = new DataContext();
            List<AppSetting> appSettings = new List<AppSetting>();
            appSettings.Add(new AppSetting { Id = 0, Key = "FileServer", Value = fileServer });
            ctx.AppSettings.AddRange(appSettings);
            ctx.SaveChanges();
            return appSettings;
        }

        public List<Check> GetChecks(List<Lessee> insertedLeeses,
            List<WellOperation> insertedWellOperations, List<Royalty> insertedroyalties, List<TblTractLessee> tblTractLessees
            )
        {
            List<Check> retval = new List<Check>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from tblRevenueReceived", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    long? fkLesseeId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkLesseeID")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeID"));
                    if (fkLesseeId != null)

                    {
                        var lessee = insertedLeeses.Where(x => x.PKLesseeId == fkLesseeId.Value).FirstOrDefault();
                        if (lessee != null)
                        {
                            string lesseeName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Company")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Company"));
                            if (string.IsNullOrEmpty(lesseeName) || string.IsNullOrWhiteSpace(lesseeName))
                            {
                                lesseeName = lessee.LesseeName;
                            }


                            retval.Add(new Check
                            {
                                Id = 0,
                                PKCheckId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkCheckID")),
                                fkLesseeID = DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeID")),
                                LesseeId = lessee.Id,
                                LesseeName = lesseeName,
                                CheckNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum")),
                                Notes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes"))),
                                ReceivedDate = DB_Reader.GetDateTime(DB_Reader.GetOrdinal("RecvDate")),
                                CheckDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("CheckDate")),
                                TotalAmount = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RecvCheckTotal")) ? 0 : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("RecvCheckTotal")),
                            });
                        }
                    }
                }
                foreach (var item in insertedroyalties.Where(x => x.CheckNum != null && x.FKWellOpsId != null))
                {
                    var wellop = insertedWellOperations.FirstOrDefault(x => x.PKWellOpsId == item.FKWellOpsId);
                    if (wellop != null && wellop.LesseeId != null)
                    {
                        retval.Add(new Check
                        {
                            Id = 0,
                            LesseeId = wellop.LesseeId.Value,
                            LesseeName = insertedLeeses.FirstOrDefault(x => x.Id == wellop.LesseeId)?.LesseeName,
                            CheckNum = item.CheckNum,
                            ReceivedDate = item.RecvDate,
                            CheckDate = item.CheckDate,
                            TotalAmount = 0,
                        });
                    }

                }
            }

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblStorRent", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    long? fkLesseeId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkLesseeID")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeID"));
                    if (fkLesseeId != null)
                    {
                        var lessee = insertedLeeses.Where(x => x.PKLesseeId == fkLesseeId.Value).FirstOrDefault();
                        if (lessee != null)
                        {
                            string checkNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum"));
                            if (!string.IsNullOrEmpty(checkNum))
                            {
                                retval.Add(new Check
                                {
                                    Id = 0,
                                    LesseeId = lessee.Id,
                                    LesseeName = lessee.LesseeName,
                                    CheckNum = checkNum,
                                    CheckDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("CheckDate")),
                                    ReceivedDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RecvDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("RecvDate")),
                                    TotalAmount = 0
                                });
                            }
                        }
                    }
                }
            }

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblRental", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    long? fkTractLessee = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkTractLessee")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkTractLessee"));
                    if (fkTractLessee != null)
                    {
                        var tblTractLessee = tblTractLessees.Where(x => x.PKTractLesseeId == fkTractLessee.Value).FirstOrDefault();
                        if (tblTractLessee != null)
                        {
                            var lessee = insertedLeeses.Where(x => x.PKLesseeId == tblTractLessee.FKLesseeId).FirstOrDefault();
                            if (lessee != null)
                            {
                                string checkNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum"));
                                if (!string.IsNullOrEmpty(checkNum))
                                {
                                    retval.Add(new Check
                                    {
                                        Id = 0,
                                        LesseeId = lessee.Id,
                                        LesseeName = lessee.LesseeName,
                                        CheckNum = checkNum,
                                        CheckDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("CheckDate")),
                                        ReceivedDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RecvDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("RecvDate")),
                                        TotalAmount = 0
                                    });
                                }
                            }
                        }
                    }
                }
            }

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblOtherDep", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    long? fkLesseeId = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkLesseeID")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeID"));
                    if (fkLesseeId != null)
                    {
                        var lessee = insertedLeeses.Where(x => x.PKLesseeId == fkLesseeId.Value).FirstOrDefault();
                        if (lessee != null)
                        {
                            string checkNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum"));
                            if (!string.IsNullOrEmpty(checkNum))
                            {
                                retval.Add(new Check
                                {
                                    Id = 0,
                                    LesseeId = lessee.Id,
                                    LesseeName = lessee.LesseeName,
                                    CheckNum = checkNum,
                                    CheckDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("CheckDate")),
                                    ReceivedDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("RecvDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("RecvDate")),
                                    TotalAmount = 0
                                });
                            }
                        }
                    }
                }
            }
            return retval.Where(x => !string.IsNullOrEmpty(x.CheckNum)).GroupBy(p => p.CheckNum).Select(g => g.First()).ToList();
        }

        private string StripHtml(string input) => Regex.Replace(input, "<.*?>", String.Empty).Replace("&nbsp;", String.Empty).Replace("&quot;", String.Empty);


        public List<Check> InsertChecks(List<Check> checks, List<Lessee> insertedLeeses)
        {
            using var ctx = new DataContext();
            ctx.Checks.AddRange(checks);
            ctx.SaveChanges();
            ExecuteNonQuery(
                @"update R
                  set R.CheckId = c.Id
                  from
                  Royalties r join Checks c on (c.CheckNum = r.CheckNum)"
                );
            return checks;
        }

        public List<long> FixLiquidRoyalties()
        {
            List<long> royaltiesToDelete = new List<long>();
            using var ctx = new DataContext();
            PaymentType paymentType = ctx.PaymentTypes.First(x => x.PaymentTypeName == "Liquids Royalty");

            List<Royalty> allRoyaltyies = ctx.Royalties
                           .Where(x => x.PaymentTypeId == paymentType.Id)
                           .ToList();

            var results = allRoyaltyies
                          .GroupBy(g => new { g.PostMonth, g.PostYear, g.WellTractInformationId })
                         .Select(g => g.First())
                         .ToList();

            foreach (var item in results)
            {
                List<Royalty> liqRoyalties = ctx.Royalties
                    .Where(x => x.WellTractInformationId == item.WellTractInformationId)
                    .Where(x => x.PaymentTypeId == paymentType.Id)
                    .Where(x => x.PostYear == item.PostYear)
                    .Where(x => x.PostMonth == item.PostMonth)
                    .OrderBy(x => x.RecvDate)
                    .ThenByDescending(x => x.LiqPayment)
                    .ToList();



                if (liqRoyalties.Count > 1)
                {
                    long royaltyId = liqRoyalties[0].Id;
                    List<RoyaltyAdjustment> raList = new List<RoyaltyAdjustment>();


                    foreach (var lr in liqRoyalties.Where(x => x.Id != royaltyId))
                    {
                        raList.Add(new RoyaltyAdjustment
                        {
                            Id = 0,
                            RoyaltyId = royaltyId,
                            CheckId = lr.CheckId,
                            EntryDate = lr.RecvDate,
                            NRI = lr.NRI,
                            Deduction = lr.Deduction,
                            LiqPayment = lr.LiqPayment,
                            LiqVolume = lr.LiqVolume
                        });
                        royaltiesToDelete.Add(lr.Id);
                    }
                    ctx.RoyaltyAdjustments.AddRange(raList);

                }
            }

            ctx.SaveChanges();
            return royaltiesToDelete;


        }

        public void DeleteRoyalties(List<long> royaltyIdstoDelete)
        {
            using var ctx = new DataContext();
            List<Royalty> royaltiesToDelete = new List<Royalty>();
            List<RoyaltyAdjustment> royaltyAdjustments = ctx.RoyaltyAdjustments.ToList();
            foreach (var item in royaltyIdstoDelete)
            {
                if (royaltyAdjustments.Where(x => x.RoyaltyId == item).FirstOrDefault() == null)
                {
                    royaltiesToDelete.Add(new Royalty { Id = item });
                }

            }

            ctx.RemoveRange(royaltiesToDelete);
            ctx.SaveChanges();
        }

        public void ExecuteNonQuery(string v)
        {
            SqlCommand cmd = new SqlCommand(v, new SqlConnection(connectionString));
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public void InsertContractsFromStorageTable(List<Lessee> insertedLeeses)
        {
            List<LandLeaseAgreement> landLeaseAgreements = new List<LandLeaseAgreement>();
            List<ContractType> contractTypes = new List<ContractType>();
            List<ContractSubType> contractSubTypes = new List<ContractSubType>();
            AltIdCategory altIdCategory = new AltIdCategory();
            using (var ctx = new DataContext())
            {
                contractTypes = ctx.ContractTypes.ToList();
                contractSubTypes = ctx.ContractSubTypes.ToList();
                altIdCategory = ctx.AltIdCategories.First(x => x.AltIdName == "Storage Field Name");
            }

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblStorage", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();
                while (DB_Reader.Read())
                {
                    List<ContractEventDetail> contractEventDetails = new List<ContractEventDetail>();
                    contractEventDetails.Add(new ContractEventDetail
                    {
                        Acres = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Acreage")) ? (Decimal?)null : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("Acreage")),
                        LesseeId = insertedLeeses.First(x => x.PKLesseeId == DB_Reader.GetInt32(DB_Reader.GetOrdinal("FKLesseeID"))).Id,
                        ActiveInd = true,
                        InterestType = "Lease",
                        LesseeName = insertedLeeses.First(x => x.PKLesseeId == DB_Reader.GetInt32(DB_Reader.GetOrdinal("FKLesseeID"))).LesseeName,
                        ShareOfLeasePercentage = 100,
                        EffectiveDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LeaseDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("LeaseDate")),
                        MinimumRoyalty = 0,
                        RoyaltyPercent = 0,
                        MinimumRoyaltySalesPrice = 0,
                    });
                    landLeaseAgreements.Add(new LandLeaseAgreement
                    {
                        Id = 0,
                        AltIdCategoryId = altIdCategory.Id,
                        AltIdInformation = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("FieldName")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("FieldName")),
                        EffectiveDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("LeaseDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("LeaseDate")),
                        TerminationDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("TerminationDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("TerminationDate")),
                        ExpirationDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ExpirationDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("ExpirationDate")),
                        Acreage = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Acreage")) ? (Decimal?)null : (decimal)DB_Reader.GetDouble(DB_Reader.GetOrdinal("Acreage")),
                        Contract = new Contract
                        {
                            Id = 0,
                            ContractNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ContractNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("ContractNum")),
                            ContractTypeId = contractTypes.First(x => x.ContractTypeName == "Land Lease").Id,
                            ContractSubTypeId = contractSubTypes.First(x => x.ContractSubTypeName == "Gas Storage Field").Id,
                            ContractNumOverride = true,
                            PaymentRequirement = new PaymentRequirement
                            {
                                Id = 0,
                            },
                            ContractEventDetails = contractEventDetails
                        }
                    }); ;

                }

            }
            using (var ctx = new DataContext())
            {
                ctx.LandLeaseAgreements.AddRange(landLeaseAgreements);
                ctx.SaveChanges();

            }
        }
        
        public List<RiderReason> InsertRiderReasons()
        {
            List<RiderReason> riderReasons = new List<RiderReason>()
            {
                new RiderReason{RiderReasonName = "Acreage"},
                new RiderReason{RiderReasonName = "Adding Wells"},
                new RiderReason{RiderReasonName = "Amount Increase"},
                new RiderReason{RiderReasonName = "Change of Address"},
                new RiderReason{RiderReasonName = "Operator Name"},
                new RiderReason{RiderReasonName = "Surety Number"},
            };
            using (var ctx = new DataContext())
            {
                ctx.RiderReasons.AddRange(riderReasons);
                ctx.SaveChanges();
                return riderReasons;
            }
        }

        public List<BondCategory> InsertBondCategories()
        {
            List<BondCategory> bondCategories = new List<BondCategory>
            {
                new BondCategory{BondCategoryName = "Performance"},
                new BondCategory{BondCategoryName = "Plugging"}
            };
            using (var ctx = new DataContext())
            {
                ctx.BondCategories.AddRange(bondCategories);
                ctx.SaveChanges();
                return bondCategories;
            }
        }

        public List<SuretyType> InsertSuretyTypes()
        {
            List<SuretyType> suretyTypes = new List<SuretyType>
            {
                new SuretyType{SuretyTypeName= "Bond"},
                new SuretyType{SuretyTypeName= "Cash"},
                new SuretyType{SuretyTypeName= "CD"},
                new SuretyType{SuretyTypeName= "Letter of Credit"},
            };
            using (var ctx = new DataContext())
            {
                ctx.SuretyTypes.AddRange(suretyTypes);
                ctx.SaveChanges();
                return suretyTypes;
            }
        }

        public List<AdditionalContractInformation> InsertAdditionalContractInformations()
        {
            List<AdditionalContractInformation> additionalContractInformations = new List<AdditionalContractInformation>
            {
                new AdditionalContractInformation{  AdditionalContractInformationName = "Exploration Agreement"},
                new AdditionalContractInformation{  AdditionalContractInformationName = "Inherited Lease"},
                new AdditionalContractInformation{  AdditionalContractInformationName = "Mitigation Agreement"},
                new AdditionalContractInformation{  AdditionalContractInformationName = "Monitoring Agreement"},
                new AdditionalContractInformation{  AdditionalContractInformationName = "Subsurface Only"},
            };
            using (var ctx = new DataContext())
            {
                ctx.AdditionalContractInformations.AddRange(additionalContractInformations);
                ctx.SaveChanges();
                return additionalContractInformations;
            }
        }

        public List<Month> InsertMonths()
        {
            List<Month> months = new List<Month>
            {
                new Month{ MonthNum = 1, MonthName = "January"},
                new Month{ MonthNum = 2, MonthName = "February" },
                new Month{ MonthNum = 3, MonthName = "March"},
                new Month{ MonthNum = 4, MonthName = "April"},
                new Month{ MonthNum = 5, MonthName = "May"},
                new Month{ MonthNum = 6, MonthName = "June"},
                new Month{ MonthNum = 7, MonthName = "July"},
                new Month{ MonthNum = 8, MonthName = "August"},
                new Month{ MonthNum = 9, MonthName = "September"},
                new Month{ MonthNum = 10, MonthName = "October"},
                new Month{ MonthNum = 11, MonthName = "November"},
                new Month{ MonthNum = 12, MonthName = "December "},
            };
            using (var ctx = new DataContext())
            {
                ctx.Months.AddRange(months);
                ctx.SaveChanges();
                return months;
            }
        }


        public List<StorageMonthLookup> GetStorageMonthLookups()
        {
            List<StorageMonthLookup> storageMonthLookups = new List<StorageMonthLookup>
            {
                new StorageMonthLookup{
                    LeaseNum = "NFGS# 38763-00",
                    BaseRentalMonths = new int[] {4 },
                },
                    new StorageMonthLookup{
                    LeaseNum = "UL098002",
                     BaseRentalMonths = new int[]  {5},
                },
                   new StorageMonthLookup{
                    LeaseNum = "NFGS# 38923",
                },
                   new StorageMonthLookup{
                    LeaseNum = "LL092827",
                       BaseRentalMonths = new int[]  {11 },
                },
                   new StorageMonthLookup{
                    LeaseNum = "LG092828",
                       BaseRentalMonths = new int[]  {11 },
                },
                     new StorageMonthLookup{
                    LeaseNum = "NFGS# 20002-01",
                     BaseRentalMonths = new int[]  {7 },
                },
                    new StorageMonthLookup{
                    LeaseNum = "NFGS# 18411-02",
                     BaseRentalMonths = new int[]  {4 },
                },
                   new StorageMonthLookup{
                    LeaseNum = "NFGS# 20223-00"
                },
                   new StorageMonthLookup{
                    LeaseNum = "NFGS# 20002-02"
                },
                   new StorageMonthLookup{
                    LeaseNum = null
                },
                            new StorageMonthLookup{
                    LeaseNum = "3173971-000",
                      BaseRentalMonths = new int[]  {1 },
                },
                    new StorageMonthLookup{
                    LeaseNum = "031137"
                },
                   new StorageMonthLookup{
                    LeaseNum = "P067446",
                    BaseRentalMonths = new int[]  {4 },
                },
                   new StorageMonthLookup{
                    LeaseNum = "NFGS# 35532-00",
                     BaseRentalMonths = new int[]  {4 },
                },
                   new StorageMonthLookup{
                    LeaseNum = "LT000067",
                       BaseRentalMonths = new int[]  {11 },
                      WellMonths = new int[]  {11 },
                },
                            new StorageMonthLookup{
                    LeaseNum = "LL000102",
                  WellMonths = new int[]  {7 },
                },
                    new StorageMonthLookup{
                    LeaseNum = "NFGS # 20033",
                    BaseRentalMonths = new int[]  {7 },
                },
                   new StorageMonthLookup{
                    LeaseNum = "P055408",
                      BaseRentalMonths = new int[]  {1 },
                        RentalMonths = new int[] {2,5,8,11 },
                    WellMonths= new int[] {1,4,7,10 }
                },
                   new StorageMonthLookup{
                    LeaseNum = "LT000278",
                    BaseRentalMonths = new int[] {6 },
                    WellMonths= new int[]{3,6,9,12 }
                },

            };
            return storageMonthLookups;
        }

        public void InsertStorageRentalPaymentTypes()
        {
            List<StorageRentalPaymentType> storageRentalPaymentTypes = new List<StorageRentalPaymentType>
            {
                 new StorageRentalPaymentType{ StorageRentalPaymentTypeName = "Base Rental"},
                new StorageRentalPaymentType{ StorageRentalPaymentTypeName = "Rental"},
                new StorageRentalPaymentType{ StorageRentalPaymentTypeName = "Well"},
                new StorageRentalPaymentType{ StorageRentalPaymentTypeName = "Credit"},
                 new StorageRentalPaymentType{ StorageRentalPaymentTypeName = "Gas Withdrawn"},
                 new StorageRentalPaymentType{ StorageRentalPaymentTypeName = "Interest"}
            };

            using (var ctx = new DataContext())
            {
                ctx.StorageRentalPaymentTypes.AddRange(storageRentalPaymentTypes);
                ctx.SaveChanges();
            }
        }

        public void InsertPeriodTypes()
        {
            List<PeriodType> periodTypes = new List<PeriodType> {
               new PeriodType{PeriodTypeName = "Annual"},
               new PeriodType{PeriodTypeName = "Bonus"},
               new PeriodType{PeriodTypeName = "Quarter 1"},
                new PeriodType{PeriodTypeName = "Quarter 2"},
                new PeriodType{PeriodTypeName = "Quarter 3"},
                 new PeriodType{PeriodTypeName = "Quarter 4"},
                  new PeriodType{PeriodTypeName = "Shut-In"},
           };
            using (var ctx = new DataContext())
            {
                ctx.PeriodTypes.AddRange(periodTypes);
                ctx.SaveChanges();
            }
        }

        public List<TblTractLessee> getTblTractLessees()
        {

            List<TblTractLessee> retval = new List<TblTractLessee>();

            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblkTractLessee", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    retval.Add(new TblTractLessee
                    {
                        PKTractLesseeId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkTractLesseeID")),
                        FKTractId = DB_Reader.GetString(DB_Reader.GetOrdinal("fkTractID")),
                        FKLesseeId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkLesseeID"))
                    });
                }

                return retval;
            }
        }
        public List<OtherRental> GetOtherRentals(List<Check> insertedChecks)
        {
            List<OtherRental> otherRentals = new List<OtherRental>();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblOtherDep", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    long? checkId = null;
                    string checkNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum"));
                    var check = insertedChecks.Where(x => x.CheckNum == checkNum).FirstOrDefault();
                    if (check != null)
                    {
                        checkId = check.Id;
                    }
                    var ordinal = DB_Reader.GetOrdinal("DepAmount");
                    double? otherRentPay = null;
                    if (!DB_Reader.IsDBNull(ordinal))
                    {
                        otherRentPay = (double)DB_Reader.GetValue(ordinal);
                    }
                    string depositType = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("DepositType")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("DepositType"));
                    DateTime? otherRentalEntryDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EntryDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EntryDate"));
                    string otherRentalNotes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes")));

                    otherRentals.Add(new OtherRental
                    {
                        Id = 0,
                        CheckId = checkId,
                        OtherRentPay = otherRentPay,
                        OtherPaymentType = depositType,
                        OtherRentalEntryDate = otherRentalEntryDate,
                        OtherRentalNotes = otherRentalNotes,
                    });
                }
            }

            return otherRentals;
        }

        public List<OtherRental> InsertOtherRentals(List<OtherRental> otherRentals)
        {
            using (var ctx = new DataContext())
            {
                ctx.OtherRentals.AddRange(otherRentals);
                ctx.SaveChanges();
            }
            return otherRentals;
        }
        public List<ContractRental> getContractRentals(List<TblTractLessee> tblTractLessees, List<Check> insertedChecks, List<Tract> insertedTracts)
        {
            List<ContractRental> contractRentals = new List<ContractRental>();
            List<Contract> contracts = new List<Contract>();
            List<PeriodType> periodTypes = new List<PeriodType>();
            using (var ctx = new DataContext())
            {
                contracts = ctx.Contracts.ToList();
                periodTypes = ctx.PeriodTypes.ToList();
            }
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblRental", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                while (DB_Reader.Read())
                {
                    long? checkId = null;
                    string checkNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("CheckNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("CheckNum"));
                    var check = insertedChecks.Where(x => x.CheckNum == checkNum).FirstOrDefault();
                    if (check != null)
                    {
                        checkId = check.Id;
                    }
                    long? fkTractLessee = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkTractLessee")) ? (long?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkTractLessee"));

                    var tblTractLessee = tblTractLessees.Where(x => fkTractLessee != null && x.PKTractLesseeId == fkTractLessee).FirstOrDefault();

                    if (tblTractLessee != null)
                    {
                        var tract = insertedTracts.Where(x => x.PKTractId == tblTractLessee.FKTractId).FirstOrDefault();
                        if (tract != null)
                        {
                            var contract = contracts.Where(x => x.TractId == tract.Id).FirstOrDefault();
                            if (contract != null)
                            {
                                string PeriodType = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PeriodType")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("PeriodType"));
                                long? periodTypeId = null;
                                PeriodType periodType = periodTypes.FirstOrDefault(x => x.PeriodTypeName == PeriodType);
                                if (periodType != null)
                                {
                                    periodTypeId = periodType.Id;
                                }
                                DateTime? contractRentalEntryDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("EntryDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("EntryDate"));
                                var ordinal = DB_Reader.GetOrdinal("RentPay");
                                decimal? contractRentPay = null;
                                if (!DB_Reader.IsDBNull(ordinal))
                                {
                                    contractRentPay = (decimal)DB_Reader.GetValue(ordinal);
                                }


                                string contractRentalNotes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes")));
                                int? contractPaymentPeriodYear = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("PostYear")) ? (int?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("PostYear"));
                                bool heldByProducton = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("heldByProduction")) ? false : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("heldByProduction"));
                                contractRentals.Add(new ContractRental
                                {
                                    Id = 0,
                                    CheckId = checkId,
                                    ContractId = contract.Id,
                                    ContractRentalEntryDate = contractRentalEntryDate,
                                    ContractRentPay = (double?)contractRentPay,
                                    ContractRentalNotes = contractRentalNotes,
                                    ContractPaymentPeriodYear = contractPaymentPeriodYear,
                                    PeriodTypeId = periodTypeId,
                                    HeldByProduction = heldByProducton

                                });
                            }
                        }
                    }

                }
            }
            return contractRentals;
        }

        public List<ContractRental> InsertContractRentals(List<ContractRental> contractRentals)
        {
            using (var ctx = new DataContext())
            {
                ctx.ContractRentals.AddRange(contractRentals);
                ctx.SaveChanges();
            }
            return contractRentals;
        }

        public List<Surety> GetSurities(
            List<Lessee> insertedLeeses, List<SuretyType> insertedSuretyTypes, List<BondCategory> insertedBondCategories,
            List<TblTractLessee> tblTractLessees, List<Tract> insertedTracts)
        {
            List<Surety> suriteis = new List<Surety>();
            using (var ctx = new DataContext()) {
                List<Contract> contracts = ctx.Contracts.ToList();
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT * from dbo_tblBond", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                    while (DB_Reader.Read())
                    {
                        long? fkTractLessee = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("fkTractLesseeID")) ? (long?)null : DB_Reader.GetInt32(DB_Reader.GetOrdinal("fkTractLesseeID"));
                        var tblTractLessee = tblTractLessees.Where(x => fkTractLessee != null && x.PKTractLesseeId == fkTractLessee).FirstOrDefault();
                        if (tblTractLessee != null)
                        {
                            var lessee = insertedLeeses.Where(x => x.PKLesseeId == tblTractLessee.FKLesseeId).FirstOrDefault();
                            var tract = insertedTracts.Where(x => x.PKTractId == tblTractLessee.FKTractId).FirstOrDefault();
                            if (lessee != null)
                            {
                                long? contractId = null;
                                if (tract != null)
                                {
                                    var contract = contracts.Where(x => x.TractId == tract.Id).FirstOrDefault();
                                    if (contract != null)
                                    {
                                        contractId = contract.Id;
                                    }
                                }
                                long? bondCategoryId = null;
                                long suretyTypeId = insertedSuretyTypes.First(x => x.SuretyTypeName == "Bond").Id;
                                string bondCategoryName = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("BondType")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("BondType"));
                                if (!string.IsNullOrEmpty(bondCategoryName) && bondCategoryName != null)
                                {
                                    var bondCategory = insertedBondCategories.FirstOrDefault(x => x.BondCategoryName == bondCategoryName);
                                    bondCategoryId = bondCategory.Id;
                                }
                                //double? currentSuretyValue = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("BondValue")) ? (double?)null : (double?)DB_Reader.GetValue(DB_Reader.GetOrdinal("BondValue"));
                                double? currentSuretyValue = null;
                                if (!DB_Reader.IsDBNull(DB_Reader.GetOrdinal("BondValue")))
                                {
                                    var csv = DB_Reader.GetValue(DB_Reader.GetOrdinal("BondValue"));
                                    currentSuretyValue = Convert.ToDouble(csv);
                                }
                                DateTime? issueDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("BondDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("BondDate"));
                                string insurer = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Insurer")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("Insurer"));
                                string suretyNum = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("BondNum")) ? null : DB_Reader.GetString(DB_Reader.GetOrdinal("BondNum"));
                                DateTime? releaseDate = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("ReleaseDate")) ? (DateTime?)null : DB_Reader.GetDateTime(DB_Reader.GetOrdinal("ReleaseDate"));
                                string suretyNotes = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Notes")) ? null : StripHtml(DB_Reader.GetString(DB_Reader.GetOrdinal("Notes")));
                                long pkBondId = DB_Reader.GetInt32(DB_Reader.GetOrdinal("pkBondID"));
                                bool released = DB_Reader.IsDBNull(DB_Reader.GetOrdinal("Released")) ? false : DB_Reader.GetBoolean(DB_Reader.GetOrdinal("Released"));


                                suriteis.Add(new Surety
                                {
                                    Id = 0,
                                    PKBondId = pkBondId,
                                    LesseeId = lessee.Id,
                                    BondCategoryId = bondCategoryId,
                                    SuretyTypeId = suretyTypeId,
                                    CurrentSuretyValue = currentSuretyValue,
                                    IssueDate = issueDate,
                                    Insurer = insurer,
                                    SuretyNum = suretyNum,
                                    ReleasedDate = releaseDate,
                                    SuretyNotes = suretyNotes,
                                    ContractId = contractId,
                                    SuretyStatus = released ? "Released" : "Active"
                                }) ; 
                            }
                        }
                    }
                }
            }
            return suriteis;
        }


        public List<Surety> InsertSurities(List<Surety> surities)
        {
            using (var ctx = new DataContext())
            {
                ctx.Sureties.AddRange(surities);
                ctx.SaveChanges();
            }
            return surities;
        }




        public void AddWellsToSurety(List<Well> insertedWells, List<Surety> insertedSurities)
        {
            List<SuretyWell> suretyWells = new List<SuretyWell>();
            using (var ctx = new DataContext())
            { 
            using (OleDbConnection con = new OleDbConnection(accessConnectionString))
            using (OleDbCommand Command = new OleDbCommand(" SELECT pkBondID, Wells.Value from dbo_tblBond where Wells.Value is not null", con))
            {
                con.Open();
                OleDbDataReader DB_Reader = Command.ExecuteReader();

                    while (DB_Reader.Read())
                    {
                        long pkBondId = DB_Reader.GetInt32(0);
                        var pkWellId = DB_Reader.GetValue(1);
                        long pkWellIdAsLong = Convert.ToInt64(pkWellId);
                        var surety = insertedSurities.First(x => x.PKBondId == pkBondId);
                        var well = insertedWells.FirstOrDefault(x => x.PKWellId == pkWellIdAsLong);
                        if (well != null)
                        {
                            suretyWells.Add(new SuretyWell
                            {
                                Id = 0,
                                WellId = well.Id,
                                SuretyId = surety.Id,
                                SuretyWellValue = surety.CurrentSuretyValue
                            });
                        }
                    }
                }
                ctx.SuretyWells.AddRange(suretyWells);
                ctx.SaveChanges();
            }
        }

        public void fixSureties()
        {
            using (var ctx = new DataContext())
            {
                List<Surety> sureties = ctx.Sureties.Include(x => x.SuretyWells).ToList();
                var suretyNumbers = sureties.Where(x => x.SuretyNum != null).Select(x => x.SuretyNum).Distinct();
                foreach (var suretyNum in suretyNumbers)
                {
                    var s = sureties.Where(x => x.SuretyNum == suretyNum);
                    if (s.Count() > 1)
                    {
                        long? bondCategoryId = null;
                        bool claimedInd = false;
                        long? contractId= null;
                        double? currentSuretyValue = null;
                        double? initialSuretyValue = null;
                        string insurer = string.Empty;
                        DateTime? issueDate = null;
                        long lesseeId = 0;
                        DateTime? releaseDate = null;
                        string suretyNotes = string.Empty;
                        long suretyTypeId = 0;
                        string suretyStatus = "Active";
                        IEnumerable<long> suretyWellIds = new List<long>();

                        foreach (var s1 in s)
                        {
                            bondCategoryId = s1.BondCategoryId;
                           claimedInd = s1.ClaimedInd;
                            contractId = s1.ContractId;
                            currentSuretyValue = s1.CurrentSuretyValue;
                            initialSuretyValue = s1.InitialSuretyValue;
                            insurer = s1.Insurer;
                            issueDate = s1.IssueDate;
                            lesseeId = s1.LesseeId;
                            releaseDate = s1.ReleasedDate;
                            suretyNotes = $"{suretyNotes} {s1.SuretyNotes}.";
                            suretyTypeId = s1.SuretyTypeId;
                            suretyStatus = s1.SuretyStatus;
                            suretyWellIds  = (s1.SuretyWells.Select(x => x.Id).ToList()).Concat(suretyWellIds);
                            foreach (var suretyWellId in suretyWellIds.Distinct())
                            {
                                var suretyWell = ctx.SuretyWells.Find(suretyWellId);
                                suretyWell.SuretyId = null;
                                ctx.SaveChanges();
                            }
                            ctx.Remove(s1);
                            ctx.SaveChanges();
                        }

                        Surety combinedSurety = new Surety
                        {
                            BondCategoryId = bondCategoryId,
                            ClaimedInd = claimedInd,
                            CurrentSuretyValue = currentSuretyValue,
                            InitialSuretyValue = initialSuretyValue,
                            Insurer = insurer,
                            IssueDate = issueDate,
                            LesseeId = lesseeId,
                            ReleasedDate = releaseDate,
                            SuretyNotes = suretyNotes,
                            SuretyTypeId = suretyTypeId,
                            SuretyNum = suretyNum,
                            SuretyStatus = suretyStatus
                        };

                        ctx.Sureties.Add(combinedSurety);
                        ctx.SaveChanges();

                        foreach (var suretyWellId in suretyWellIds.Distinct())
                        {
                            var suretyWell= ctx.SuretyWells.Find(suretyWellId);
                            suretyWell.SuretyId = combinedSurety.Id;
                            ctx.SaveChanges();
                        }
                    }
                }              
            }
        }

        public void AddAltIdsToWells(List<Well> insertedWells, List<WellOperation> insertedWellOperations, List<WellOpsAltIdLookup> wellOpsAltIdLookups)
        {
            using (var ctx = new DataContext())
            {
                foreach (var item in wellOpsAltIdLookups)
                {
                    WellOperation wellOperation = insertedWellOperations.Where(x => x.PKWellOpsId == item.PKWellOpsId).FirstOrDefault();
                    if (wellOperation != null && wellOperation.WellId != null)
                    {
                        var well = ctx.Wells.Find(wellOperation.WellId.Value);
                        well.AltId = item.PropertyNumber.ToString();
                        well.AltIdType = "Operator ID";
                    }
                }
                ctx.SaveChanges();
            }
        }
    }
}

