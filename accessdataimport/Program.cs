using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace accessdataimport
{
    class Program
    {
        static void Main()
        {
            //get the Service through DI
            var collection = new ServiceCollection();
            collection.AddScoped<IDataImportService, DataImportService>();
            var serviceProvider = collection.BuildServiceProvider();
            var service = serviceProvider.GetService<IDataImportService>();

            //load the config file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            Console.WriteLine("Get Storage Lookup Months");
            List<StorageMonthLookup> storageMonthLookups = service.GetStorageMonthLookups();
            Console.WriteLine("deleteData");
            service.deleteData();
            Console.WriteLine("insert months");
            List<Month> insertedMonths = service.InsertMonths();
            Console.WriteLine("Insert Additional Contract Informations");
            List<AdditionalContractInformation> insertedAdditionalContractInformations = service.InsertAdditionalContractInformations();
            Console.WriteLine("Insert Bond Categories");
            List<BondCategory> insertedBondCategories = service.InsertBondCategories();
            Console.WriteLine("Insert Rider Reasons");
            List<RiderReason> insertedRiderReasons = service.InsertRiderReasons();
            Console.WriteLine("Insert Surety Types");
            List<SuretyType> insertedSuretyTypes = service.InsertSuretyTypes();
            Console.WriteLine("InsertWellStatuses");
            List<WellStatus> wellStatuses = service.InsertWellStatuses();
            Console.WriteLine("InsertPaymentTypes");
            List<PaymentType> paymentTypes = service.InsertPaymentTypes();
            Console.WriteLine("InsertProductTypes");
            List<ProductType> productTypes = service.InsertProductTypes();
            Console.WriteLine("WellTypes");
            List<WellType> wellTypes = service.InsertWellTypes();
            Console.WriteLine("InsertWellLogTestTypes");
            List<WellLogTestType> wellLogTestTypes = service.InsertWellLogTestTypes();
            Console.WriteLine("InsertWellStatusLookups");
            List<WellStatusLookup> wellStatusLookups = service.InsertWellStatusLookups();
            Console.WriteLine("GetTblTractOptShares");
            List<TblTractOptShare> TblTractOptShares = service.GetTblTractOptShares();
            Console.WriteLine("InsertAltIdCategories");
            List<AltIdCategory> insertedAltIdCategories = service.InsertAltIdCategories();
            Console.WriteLine("InsertTerminationReason");
            List<TerminationReason> insertedTerminationReasons = service.InsertTerminationReasons();
            Console.WriteLine("GetWellOperator");
            List<WellOperator> wellOperators = service.GetWellOperators();
            Console.WriteLine("GetWellOpsAltIdLookup");
            List<WellOpsAltIdLookup> wellOpsAltIdLookups  = service.GetWellOpsAltIdLookups();
            Console.WriteLine("GetNonUnitzedWellAcreage");
            List<NonUnitizedWellsAcre> nonUnitizedWellsAcres = service.GetNonUnitizedWellAcres();
            Console.WriteLine("GetTownShips");
            List<Township> townships = service.GetTownShips();
            Console.WriteLine("GetLesseeNameChangeExcelFil");
            List<LesseeNameChangeExcelFile> lesseeNameChangeExcelFiles = service.GetLesseeNameChangeExcelFile();
            Console.WriteLine("GetAltContractExcelFile");
            List<AltContractExcel> altContractExcelFiles = service.GetAltContractExcelFile();
            Console.WriteLine("InsertTownships");
            List<Township> insertedTownships = service.InsertTownships(townships);
            Console.WriteLine("GetStates");
            List<State> states = service.GetStates();
            Console.WriteLine("InsertStates");
            List<State> insertedStates = service.InsertStates(states);
            Console.WriteLine("GetApiCodes");
            List<ApiCode> apiCodes = service.GetApiCodes();
            Console.WriteLine("InsertApiCodes");
            List<ApiCode> insertedApiCodes = service.InsertApiCodes(apiCodes);
            Console.WriteLine("GetFormations");
            List<Formation> formations = service.GetFormations();
            Console.WriteLine("InsertFormations");
            List<Formation> insertedFormations = service.InsertFormations(formations);
            Console.WriteLine("GetDistricts");
            List<District> districts = service.GetDistricts();
            Console.WriteLine("InsertDistricts");
            List<District> insertedDistricts = service.InsertDistricts(districts);
            Console.WriteLine("InsertContractTypes");
            List<ContractType> contractTypes = service.InsertContractTypes();
            Console.WriteLine("InsertcontractEventDetailReasonsForChange");
            List<ContractEventDetailReasonForChange> contractEventDetailReasonsForChange = service.InsertcontractEventDetailReasonsForChange();

            Console.WriteLine("GetLessees");
            List<Lessee> lessees = service.GetLessees();
            Console.WriteLine("InsertLessees");
            List<Lessee> insertedLeeses = service.InsertLessees(lessees);

            Console.WriteLine("GetUnits");
            List<Unit> units = service.GetUnits();
            Console.WriteLine("InsertUnits");
            List<Unit> insertedUnits = service.InsertUnits(units);

            Console.WriteLine("GetTracts");
            List<Tract> tracts = service.GetTracts(insertedDistricts, insertedLeeses);
            Console.WriteLine("InsertTracts");
            List<Tract> insertedTracts = service.InsertTracts(tracts);
            Console.WriteLine("InsertDistrictTractJunctions");
            List<DistrictTractJunction> insertedDistrictTractJunctions = service.InsertDistrictTractJunctions(insertedTracts);
            Console.WriteLine("GetTractLesseeJunctions");
            List <TractLesseeJunction> tractLesseeJunctions = service.GetTractLesseeJunctions(insertedTracts, insertedLeeses);
            Console.WriteLine("InsertTractLesseeJuntions");
            List<TractLesseeJunction> insertedTractLesseeJunctions = service.InsertTractLesseeJuntions(tractLesseeJunctions);
            Console.WriteLine("GetPads");
            List<Pad> pads = service.GetPads(insertedTracts);
            List<Pad> insertedPads = service.InsertPads(pads);
            Console.WriteLine("insertedRoles");
            List<Role> insertedRoles = service.InsertRoles();

            Console.WriteLine("getSurfaceUseAgreements");
            List<SurfaceUseAgreement> surfaceUseAgreements = service.getSurfaceUseAgreements(contractTypes, insertedTracts, insertedLeeses);
            Console.WriteLine("insertSurfaceUseAgreements");
            List<SurfaceUseAgreement> insertedSurfaceUseAgreements = service.insertSurfaceUseAgreements(surfaceUseAgreements, insertedTracts, contractTypes);
            Console.WriteLine("insertLandLeaseAgreement");
            List<LandLeaseAgreement> insertedLandLeaseAgreements = service.insertLandLeaseAgreements(insertedTracts, contractTypes);
            Console.WriteLine("insertProspectingAgreements");
            List<ProspectingAgreement> insertedProspectingAgreements = service.insertProspectingAgreements(insertedTracts, contractTypes);
            Console.WriteLine("insertDistrictContractJunctions");
            List<DistrictContractJunction> insertedDistrictContractJunctions = service.insertDistrictContractJunctions();
            Console.WriteLine("get tblTractLessee");
            List<TblTractLessee> tblTractLessees = service.getTblTractLessees();
            Console.WriteLine("InsertContractEventDetails");
            List<ContractEventDetail> insertedContractEventDetails = service.InsertContractEventDetails(
                insertedTracts, TblTractOptShares, tblTractLessees);
            Console.WriteLine("AddLesseeHistoricalRecords");
            service.AddLesseeHistoricalRecords(lesseeNameChangeExcelFiles, insertedLeeses);

            Console.WriteLine("wells");
            List<RevenueReceived> revenueReceivedList = service.GetRevenueReceived(insertedLeeses);

            Console.WriteLine("InsertRevenuesReceived");
            List<RevenueReceived> insertedRevuesReceived = service.InsertRevenuesReceived(revenueReceivedList);

            Console.WriteLine("GetWells");
            List<Well> wells = service.GetWells(
                insertedPads, wellStatuses, wellStatusLookups, insertedLeeses, wellOperators, altContractExcelFiles, insertedAltIdCategories, insertedFormations, nonUnitizedWellsAcres);

            Console.WriteLine("InsertWells");
            List<Well> insertedWells = service.InsertWells(wells);

            Console.WriteLine("GetWellOperations");
            List<WellOperation> wellOperations = service.GetWellOperations(insertedWells, insertedLeeses);

            Console.WriteLine("InsertWellOperations");
            List<WellOperation> insertedWellOperations = service.InsertWellOperations(wellOperations);

            Console.WriteLine("InsertWellTractInformation");
            List<WellTractInformation> insertedWellTractInformation =
                service.InsertWellTractInformation(insertedWells, insertedWellOperations, insertedLeeses, insertedTracts);

            Console.WriteLine("GetRoyalties");
            List<Royalty> royalties = service.GetRoyalties(insertedRevuesReceived, paymentTypes, productTypes);

            Console.WriteLine("InsertRoyalties");
            List<Royalty> insertedroyalties = service.InsertRoyalties(royalties);

            Console.WriteLine("UpdateRoyalties");
            insertedroyalties = service.UpdateRoyalties(insertedroyalties, insertedWellTractInformation);

            Console.WriteLine("InsertTractUnitJunctions");
            List<TractUnitJunction> insertedTractUnitJunctions = service.InsertTractUnitJunctions(insertedUnits, insertedTracts, insertedWells);

            Console.WriteLine("InsertAppSettings");
            List<AppSetting> insertedAppSettings = service.InsertAppSettings();

            Console.WriteLine("insertedLeeses");
            List<Check> checks = service.GetChecks(insertedLeeses, insertedWellOperations, insertedroyalties, tblTractLessees);

            Console.WriteLine("insertedChecks");
            List<Check> insertedChecks = service.InsertChecks(checks, insertedLeeses);

            Console.WriteLine("GetRoyaltyAdustments");
            List<RoyaltyAdjustment> royaltyAdjustments = service.GetRoyaltyAdustments(insertedroyalties, insertedChecks);
            Console.WriteLine("InsertRoyaltyAdjustments");
            List<RoyaltyAdjustment> insertedroyaltyAdjustments = service.InsertRoyaltyAdjustments(royaltyAdjustments);
            Console.WriteLine("fix liquid royaltyies");
            List<long> royaltyIdstoDelete = service.FixLiquidRoyalties();
            Console.WriteLine("deleting royaltyies");
            service.DeleteRoyalties(royaltyIdstoDelete);
            Console.WriteLine("insert contracts from storage table");    
            service.InsertContractsFromStorageTable(insertedLeeses);
            Console.WriteLine("insert storage rental types");
            service.InsertStorageRentalPaymentTypes();
            Console.WriteLine("insert period Types");
            service.InsertPeriodTypes();
            Console.WriteLine("get storage");
            List<Storage> storages = service.GetStorages(insertedLeeses, storageMonthLookups);
            Console.WriteLine("insert storage");
            List<Storage> insertedStorages = service.insertStorages(storages);
            Console.WriteLine("get contract rentals");
            List<ContractRental> contractRentals = service.getContractRentals(tblTractLessees, insertedChecks, insertedTracts);
            Console.WriteLine("insert contract rentals");
            List<ContractRental> insertedContractRentals = service.InsertContractRentals(contractRentals);
            Console.WriteLine("get other rentals");
            List<OtherRental> otherRentals = service.GetOtherRentals(insertedChecks);
            Console.WriteLine("insert other rentals");
            List<OtherRental> insertedOtherRentals = service.InsertOtherRentals(otherRentals);
            Console.WriteLine("insert surities");
            List<Surety> surities = service.GetSurities(insertedLeeses, insertedSuretyTypes, insertedBondCategories, tblTractLessees, insertedTracts);
            List<Surety> insertedSurities = service.InsertSurities(surities);
            Console.WriteLine("add wells to surety");
            service.AddWellsToSurety(insertedWells, insertedSurities);
            Console.WriteLine("fix sureties");
            service.fixSureties();
            Console.WriteLine("add altId to wells");
            service.AddAltIdsToWells(insertedWells, insertedWellOperations, wellOpsAltIdLookups);

        }
    }
}
