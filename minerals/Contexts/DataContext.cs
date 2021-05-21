using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minerals.ViewModels;
using Models;

namespace Minerals.Contexts
{
    public class DataContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<CwopaAgencyFile> CwopaAgencyFiles { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<ADDepUser> ADDepUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleJunction> UserRoleJunctions { get; set; }
        public DbSet<RevenueReceived> RevenueReceived { get; set; }
        public DbSet<Lessee> Lessees { get; set; }
        public DbSet<Tract> Tracts { get; set; }
        public DbSet<TractLesseeJunction> TractLesseeJunctions { get; set; }
        public DbSet<Pad> Pads { get; set; }
        public DbSet<Well> Wells { get; set; }
        public DbSet<Royalty> Royalties { get; set; }
        public DbSet<RoyaltyAdjustment> RoyaltyAdjustments { get; set; }
        public DbSet<WellOperation> WellOperations { get; set; }
        public DbSet<RoyaltyAdjustmentCardViewModel> RoyaltyAdjustmentCardViewModels { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<ContractSubType> ContractSubTypes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<SurfaceUseAgreement> SurfaceUseAgreements { get; set; }
        public DbSet<LandLeaseAgreement> LandLeaseAgreements { get; set; }
        public DbSet<ProspectingAgreement> ProspectingAgreements { get; set; }
        public DbSet<ProductionAgreement> ProductionAgreements { get; set; }
        public DbSet<SeismicAgreement> SeismicAgreements { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<DistrictContractJunction> DistrictContractJunctions { get; set; }
        public DbSet<DistrictTractJunction> DistrictTractJunctions { get; set; }
        public DbSet<Township> Townships { get; set; }
        public DbSet<TerminationReason> TerminationReasons { get; set; }
        public DbSet<TownshipLandLeaseAgreementJunction> TownshipLandLeaseAgreementJunctions { get; set; }
        public DbSet<TownshipSurfaceUseAgreementJunction> TownshipSurfaceUseAgreementJunctions { get; set; }
        public DbSet<TownshipProspectingAgreementJunction> TownshipProspectingAgreementJunctions { get; set; }
        public DbSet<TownshipProductionAgreementJunction> TownshipProductionAgreementJunctions { get; set; }
        public DbSet<TownshipSeismicAgreementJunction> TownshipSeismicAgreementJunctions { get; set; }
        public DbSet<AltIdCategory> AltIdCategories { get; set; }
        public DbSet<RowContract> RowContracts { get; set; }
        public DbSet<PaymentRequirement> PaymentRequirements { get; set; }
        public DbSet<PluggingSuretyDetail> PluggingSuretyDetails { get; set; }
        public DbSet<AssociatedContract> AssociatedContracts { get; set; }
        public DbSet<AssociatedTract> AssociatedTracts { get; set; }
        public DbSet<LesseeContact> LesseeContacts { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<LesseeHistory> LesseeHistories { get; set; }
        public DbSet<ContractEventDetail> ContractEventDetails { get; set; }
        public DbSet<ContractEventDetailReasonForChange> ContractEventDetailReasonsForChange { get; set; }
        public DbSet<WellStatus> WellStatuses { get; set; }
        public DbSet<ApiCode> ApiCodes { get; set; }
        public DbSet<WellType> WellTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitGroup> UnitGroups { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<WellTractInformation> WellTractInformations { get; set; }
        public DbSet<WellLogTestType> WellLogTestTypes { get; set; }
        public DbSet<DigitalWellLogTestTypeWellJunction> DigitalWellLogTestTypeWellJunctions { get; set; }
        public DbSet<DigitalImageWellLogTestTypeWellJunction> DigitalImageWellLogTestTypeWellJunctions { get; set; }
        public DbSet<HardCopyWellLogTestTypeWellJunction> HardCopyWellLogTestTypeWellJunctions { get; set; }
        public DbSet<TractUnitJunction> TractUnitJunctions { get; set; }
        public DbSet<TractUnitJunctionWellJunction> TractUnitJunctionWellJunctions { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<WellboreShare> WellBoreShares { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<StorageRental> StorageRentals { get; set; }
        public DbSet<ContractRental> ContractRentals { get; set; }
        public DbSet<OtherRental> OtherRentals { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<StorageWellPaymentMonthJunction> StorageWellPaymentMonthJunctions { get; set; }
        public DbSet<StorageBaseRentalPaymentMonthJunction> StorageBaseRentalPaymentMonthJunctions { get; set; }
        public DbSet<StorageRentalPaymentMonthJunction> StorageRentalPaymentMonthJunctions { get; set; }
        public DbSet<StorageRentalPaymentType> StorageRentalPaymentTypes { get; set; }
        public DbSet<PeriodType> PeriodTypes { get; set; }
        public DbSet<AdditionalBonus> AdditionalBonuses { get; set; }
        public DbSet<ContractRentalPaymentMonthJunction> ContractRentalPaymentMonthJunctions { get; set; }
        public DbSet<UploadTemplate> UploadTemplates { get; set; }
        public DbSet<UploadTemplateMappedHeader> UploadTemplateMappedHeaders { get; set; }
        public DbSet<UploadTemplateUnmappedHeader> UploadTemplateUnmappedHeaders { get; set; }
        public DbSet<UploadPayment> UploadPayments { get; set; }
        public DbSet<AdditionalContractInformation> AdditionalContractInformations { get; set; }
        public DbSet<BondCategory> BondCategories { get; set; }
        public DbSet<SuretyType> SuretyTypes { get; set; }
        public DbSet<Surety> Sureties { get; set; }
        public DbSet<CSVPayment> CSVPayments { get; set; }
        public DbSet<RiderReason> RiderReasons { get; set; }
        public DbSet<SuretyRider> SuretyRiders { get; set; }
        public DbSet<SuretyWell> SuretyWells { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //make sure in well operations the combo between lessee id and well id is unique
            modelBuilder.Entity<WellOperation>()
                .HasIndex(p => new { p.LesseeId, p.WellId }).IsUnique();

            modelBuilder.Entity<RoyaltyAdjustmentCardViewModel>().ToView("RoyaltyAdjustmentCardViewModel").HasNoKey();

            //unique constraint on user name
            modelBuilder.Entity<Role>()
                .HasAlternateKey(c => c.RoleName);

        }
    }
}
