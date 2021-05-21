using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Minerals.BusinessLogic;
using Minerals.Contexts;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Minerals.Repositories;
using Minerals.ViewModels;
using System.IO;

namespace Minerals
{
    public class Startup
    {
        public Startup(IConfiguration config) => Configuration = config;
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddTransient<ICwopaAgencyFileRepository, CwopaAgencyFileRepository>();
            services.AddTransient<ICurrentUserRepository, CurrentUserRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRevenueReceivedRepository, RevenueReceivedRepository>();
            services.AddTransient<ITractLesseeJunctionRepository, TractLesseeJunctionRepository>();
            services.AddTransient<IPadRepository, PadRepository>();
            services.AddTransient<IWellRepository, WellRepository>();
            services.AddTransient<IRoyaltyRepository, RoyaltyRepository>();
            services.AddTransient<IRoyaltyAdjustmentCardViewModelRepository, RoyaltyAdjustmentCardViewModelRepository>();
            services.AddTransient<ITractRepository, TractRepository>();
            services.AddTransient<ITownshipRepository, TownshipRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IContractBusinessLogic, ContractBusinessLogic>();
            services.AddTransient<IStorageRentalBusinessLogic, StorageRentalBusinessLogic>();
            services.AddTransient<IContractRentalBusinessLogic, ContractRentalBusinessLogic>();
            services.AddTransient<IOtherPaymentBusinessLogic, OtherPaymentBusinessLogic>();
            services.AddTransient<IUnitBusinessLogic, UnitBusinessLogic>();
            services.AddTransient<ILesseeContactBusinessLogic, LesseeContactBusinessLogic>();
            services.AddTransient<IWellBusinessLogic, WellBusinessLogic>();
            services.AddTransient<ITractBusinessLogic, TractBusinessLogic>();
            services.AddTransient<ILesseeBusinessLogic, LesseeBusinessLogic>();
            services.AddTransient<ICheckBusinessLogic, CheckBusinessLogic>();
            services.AddTransient<IPaymentBusinessLogic, PaymentBusinessLogic>();
            services.AddTransient<ISuretyBusinessLogic, SuretyBusinessLogic>();
            services.AddTransient<IRowContractRepository, RowContractRepository>();
            services.AddTransient<IPaymentRequirementRepository, PaymentRequirementRepository>();
            services.AddTransient<IPluggingSuretyDetailRepository, PluggingSuretyDetailRepository>();
            services.AddTransient<IAssociatedContractRepository, AssociatedContractRepository>();
            services.AddTransient<IAssociatedTractRepository, AssociatedTractRepository>();
            services.AddTransient<ILandLeaseAgreementRepository, LandLeaseAgreementRepository>();
            services.AddTransient<ISurfaceUseAgreementRepository, SurfaceUseAgreementRepository>();
            services.AddTransient<IProspectingAgreementRepository, ProspectingAgreementRepository>();
            services.AddTransient<IProductionAgreementRepository, ProductionAgreementRepository>();
            services.AddTransient<ISeismicAgreementRepository, SeismicAgreementRepository>();
            services.AddTransient<ILesseeContactRepository, LesseeContactRepository>();
            services.AddTransient<ILesseeRepository, LesseeRepository>();
            services.AddTransient<ILesseeHistoryRepository, LesseeHistoryRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<IContractEventDetailRepository, ContractEventDetailRepository>();
            services.AddTransient<IApiCodeRepository, ApiCodeRepository>();
            services.AddTransient<IWellTractInformationRepository, WellTractInformationRepository>();
            services.AddTransient<IUnitRepository, UnitRepository>();
            services.AddTransient<ITractUnitJunctionRepository, TractUnitJunctionRepository>();
            services.AddTransient<ITractUnitJunctionWellJunctionRepository, TractUnitJunctionWellJunctionRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IWellBoreShareRepository, WellBoreShareRepository>();
            services.AddTransient<ICheckRepository, CheckRepository>();
            services.AddTransient<IRoyaltyAdjustmentRepository, RoyaltyAdjustmentRepository>();
            services.AddTransient<IContractRentalRepository, ContractRentalRepository>();
            services.AddTransient<IOtherPaymentRepository, OtherPaymentRepsository>();
            services.AddTransient<IStorageRentalRepository, StorageRentalRepository>();
            services.AddTransient<IStorageRepository, StorageRepository>();
            services.AddTransient<IAdditionalBonusRepository, AdditionalBonusRepository>();
            services.AddTransient<IStorageWellPaymentMonthJunctionRepository, StorageWellPaymentMonthJunctionRepository>();
            services.AddTransient<IStorageBaseRentalPaymentMonthJunctionRepository, StorageBaseRentalPaymentMonthJunctionRepository>();
            services.AddTransient<IStorageRentalPaymentMonthJunctionRepository, StorageRentalPaymentMonthJunctionRepository>();
            services.AddTransient<IContractRentalPaymentMonthJunctionRepository, ContractRentalPaymentMonthJunctionRepository>();
            services.AddTransient<IUploadTemplateBusinessLogic, UploadTemplateBusinessLogic>();
            services.AddTransient<IUploadTemplateRepository, UploadTemplateRepository>();
            services.AddTransient<IUploadPaymentRepository, UploadPaymentRepository>();
            services.AddTransient<IUploadPaymentBusinessLogic, UploadPaymentBusinessLogic>();
            services.AddTransient<ISuretyRepository, SuretyRepository>();
            services.AddSingleton<MenuDataViewModel>();
            services.AddSingleton<FileTypeData>();
            services.AddScoped<ErrorLoggingAttribute>();
            string conString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(conString));
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddMvcOptions(options =>
                {
                    options.Filters.AddService(typeof(ErrorLoggingAttribute))
                    ;
                });
            services.AddMemoryCache();
            services.AddSession();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",new OpenApiInfo
                {
                    Title = "Minerals Application",
                    Version = "v1",
                    Description = "List of api services used by the minerals application",
                    Contact = new OpenApiContact { Name = "Bryan Dellinger", Email = "c-bdelling@pa.gov"},
                });
            });

            var section = Configuration.GetSection(nameof(ClientConfig));
            var clientConfig = section.Get<ClientConfig>();
            services.AddSingleton(clientConfig);
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
                RequestPath = new PathString("/images")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
                RequestPath = new PathString("/images")
            });
            app.UseSession();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "My API");
            });
        }
    }
}
