using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace accessdataimport
{
    public interface IDataImportService
    {
        List<RevenueReceived> GetRevenueReceived(List<Lessee> lessees);
        List<RevenueReceived> InsertRevenuesReceived(List<RevenueReceived> revenuesReceived);
        List<Lessee> GetLessees();
        List<Lessee> InsertLessees(List<Lessee> lessees);
        void deleteData();
        List<Tract> GetTracts(List<District> insertedDistricts, List<Lessee> insertedLeeses);
        List<Tract> InsertTracts(List<Tract> tracts);
        List<TractLesseeJunction> GetTractLesseeJunctions(List<Tract> insertedTracts, List<Lessee> insertedLeeses);
        List<TractLesseeJunction> InsertTractLesseeJuntions(List<TractLesseeJunction> tractLesseeJunctions);
        List<Pad> GetPads(List<Tract> insertedTracts);
        List<Pad> InsertPads(List<Pad> pads);
        List<WellStatus> InsertWellStatuses();
        List<StorageMonthLookup> GetStorageMonthLookups();
        List<Township> GetTownShips();
        List<PaymentType> InsertPaymentTypes();
        List<ProductType> InsertProductTypes();
        List<WellType> InsertWellTypes();
        List<WellStatusLookup> InsertWellStatusLookups();
        List<WellLogTestType> InsertWellLogTestTypes();
        List<TblTractOptShare> GetTblTractOptShares();
        List<AdditionalContractInformation> InsertAdditionalContractInformations();
        List<TerminationReason> InsertTerminationReasons();
        List<BondCategory> InsertBondCategories();
        List<Well> GetWells(List<Pad> insertedPads, List<WellStatus> wellStatuses, List<WellStatusLookup> wellStatusLookups, List<Lessee> insertedLessees, List<WellOperator> wellOperators, List<AltContractExcel> altContractExcelFiles, List<AltIdCategory> insertedAltIdCategories, List<Formation> insertedFormations, List<NonUnitizedWellsAcre> nonUnitizedWellsAcres);
        List<RiderReason> InsertRiderReasons();
        List<SuretyType> InsertSuretyTypes();
        List<Township> InsertTownships(List<Township> townships);
        List<WellOperator> GetWellOperators();
        List<District> GetDistricts();
        List<ContractType> InsertContractTypes();
        List<LesseeNameChangeExcelFile> GetLesseeNameChangeExcelFile();
        List<District> InsertDistricts(List<District> districts);
        List<AltContractExcel> GetAltContractExcelFile();
        List<State> GetStates();
        List<State> InsertStates(List<State> states);
        List<Well> InsertWells(List<Well> wells);
        List<ApiCode> GetApiCodes();
        List<Royalty> GetRoyalties(List<RevenueReceived> insertedRevuesReceived, List<PaymentType> paymentTypes, List<ProductType> productTypes);
        List<Formation> GetFormations();
        List<Formation> InsertFormations(List<Formation> formations);
        List<ApiCode> InsertApiCodes(List<ApiCode> apiCodes);
        List<Royalty> InsertRoyalties(List<Royalty> royalties);
        List<NonUnitizedWellsAcre> GetNonUnitizedWellAcres();
        List<WellOperation> GetWellOperations(List<Well> insertedWells, List<Lessee> insertedLeeses);
        List<ContractEventDetailReasonForChange> InsertcontractEventDetailReasonsForChange();
        List<WellOpsAltIdLookup> GetWellOpsAltIdLookups();
        List<WellOperation> InsertWellOperations(List<WellOperation> wellOperations);
        List<Unit> InsertUnits(List<Unit> units);
        List<Unit> GetUnits();
        List<Role> InsertRoles();
        List<SurfaceUseAgreement> getSurfaceUseAgreements(List<ContractType> contractTypes, List<Tract> tracts, List<Lessee> lessees);
        List<SurfaceUseAgreement> insertSurfaceUseAgreements(List<SurfaceUseAgreement> surfaceUseAgreements, List<Tract> tracts, List<ContractType> contractTypes);
        List<DistrictTractJunction> InsertDistrictTractJunctions(List<Tract> insertedTracts);
        List<LandLeaseAgreement> insertLandLeaseAgreements(List<Tract> insertedTracts, List<ContractType> contractTypes);
        List<ProspectingAgreement> insertProspectingAgreements(List<Tract> insertedTracts, List<ContractType> contractTypes);
        List<DistrictContractJunction> insertDistrictContractJunctions();
        List<AltIdCategory> InsertAltIdCategories();
        List<ContractEventDetail> InsertContractEventDetails(List<Tract> insertedTracts, List<TblTractOptShare> tblTractOptShares, List<TblTractLessee> tblTractLessees);
        void AddLesseeHistoricalRecords(List<LesseeNameChangeExcelFile> lesseeNameChangeExcelFiles, List<Lessee> insertedLeeses);
        List<WellTractInformation> InsertWellTractInformation(List<Well> insertedWells, List<WellOperation> insertedWellOperations, List<Lessee> insertedLeeses, List<Tract> insertedTracts);
        List<TractUnitJunction> InsertTractUnitJunctions(List<Unit> insertedUnits, List<Tract> insertedTracts, List<Well> insertedWells);
        List<AppSetting> InsertAppSettings();
        List<Check> GetChecks(List<Lessee> insertedLeeses, List<WellOperation> insertedWellOperations, List<Royalty> insertedroyalties, List<TblTractLessee> tblTractLessees);
        List<Check> InsertChecks(List<Check> checks, List<Lessee> insertedLeeses);
        List<Royalty> UpdateRoyalties(List<Royalty> insertedroyalties, List<WellTractInformation> insertedWellTractInformation);
        List<RoyaltyAdjustment> GetRoyaltyAdustments(List<Royalty> insertedroyalties, List<Check> insertedChecks);
        List<RoyaltyAdjustment> InsertRoyaltyAdjustments(List<RoyaltyAdjustment> royaltyAdjustments);
        List<long> FixLiquidRoyalties();
        void DeleteRoyalties(List<long> royaltyIdstoDelete);
        void InsertContractsFromStorageTable(List<Lessee> insertedLeeses);
        List<Storage> GetStorages(List<Lessee> insertedLeeses, List<StorageMonthLookup> storageMonthLookups);
        List<Storage> insertStorages(List<Storage> storages);
        List<Month> InsertMonths();
        void InsertStorageRentalPaymentTypes();
        void InsertPeriodTypes();
        List<TblTractLessee> getTblTractLessees();
        List<ContractRental> getContractRentals(List<TblTractLessee> tblTractLessees, List<Check> insertedChecks, List<Tract> insertedTracts);
        List<ContractRental> InsertContractRentals(List<ContractRental> contractRentals);
        List<OtherRental> GetOtherRentals(List<Check> insertedChecks);
        List<OtherRental> InsertOtherRentals(List<OtherRental> otherRentals);
        List<Surety> GetSurities(List<Lessee> insertedLeeses, List<SuretyType> insertedSuretyTypes, List<BondCategory> insertedBondCategories, List<TblTractLessee> tblTractLessees, List<Tract> insertedTracts);
        List<Surety> InsertSurities(List<Surety> surities);
        void AddWellsToSurety(List<Well> insertedWells, List<Surety> insertedSurities);
        void fixSureties();
        void AddAltIdsToWells(List<Well> insertedWells, List<WellOperation> insertedWellOperations, List<WellOpsAltIdLookup> wellOpsAltIdLookups);
    }
}
