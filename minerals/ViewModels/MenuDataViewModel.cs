using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class MenuDataViewModel
    {
        private MenuViewModel Menu = new MenuViewModel
        {
           Modules = new List<ModuleViewModel>
           {
            
                new ModuleViewModel
                {
                    isAdmin = false,
                    ModuleName = "Contracts",
                    Icon = "fas fa-file-contract",
                    CardColor = "bg-success",
                    TextColor = "text-white",
                    Pages = new List<ModulePageViewModel>
                    {
                         new ModulePageViewModel{AspRouteContainer = null, PageName = "Search Contracts" },
                         new ModulePageViewModel{AspRouteContainer = "addUpdateContractContainer", PageName = "Add Contract" },
                         new ModulePageViewModel{AspRouteContainer = "manageLesseesContainer", PageName = "Lessees" },
                         new ModulePageViewModel{AspRouteContainer = "manageTractsContainer", PageName = "Tracts" },
                    }
                },
                 new ModuleViewModel
                {
                    isAdmin = false,
                    ModuleName = "Wells",
                    Icon = "fas fa-atom",
                    CardColor = "bg-light",
                    TextColor = "text-dark",
                    Pages = new List<ModulePageViewModel>
                    {
                         new ModulePageViewModel{AspRouteContainer = null, PageName = "Search Wells" },
                         new ModulePageViewModel{AspRouteContainer = "addUpdateWellContainer", PageName = "Add Well" },
                    }
                },
                 new ModuleViewModel
                 {
                     isAdmin = false,
                     ModuleName = "Units",
                     Icon = "fas fa-cubes",
                     CardColor = "bg-dark",
                     TextColor = "text-white",
                     Pages = new List<ModulePageViewModel>
                    {
                          new ModulePageViewModel{AspRouteContainer = null, PageName = "Search Units" },
                          new ModulePageViewModel{AspRouteContainer = "addUpdateUnitContainer", PageName = "Add Unit" },
                    }

                 },
                  new ModuleViewModel
                 {
                     isAdmin = false,
                     ModuleName = "Accounting",
                     Icon = "fas fa-calculator",
                     CardColor = "bg-info",
                     TextColor = "text-white",
                     Pages = new List<ModulePageViewModel>
                    {
                          new ModulePageViewModel{AspRouteContainer = null, PageName = "Search Checks" },
                          new ModulePageViewModel{AspRouteContainer = "addUpdateCheckContainer", PageName = "Add Check" },
                          new ModulePageViewModel{AspRouteContainer = "managePaymentsContainer", PageName = "Search Royalty Payments" },
                          new ModulePageViewModel{AspRouteContainer = "addUpdatePaymentContainer", PageName = "Add Royalty Payment" },
                          new ModulePageViewModel{AspRouteContainer = "manageStorageFieldsContainer", PageName = "Search Storage Field Payments" },
                          new ModulePageViewModel{AspRouteContainer = "addUpdateStorageFieldContainer", PageName = "Add Storage Field Payment" },
                          new ModulePageViewModel{AspRouteContainer = "manageContractRentalsContainer", PageName = "Search Contract Rental Payments" },
                          new ModulePageViewModel{AspRouteContainer = "addUpdateContractRentalContainer", PageName = "Add Contract Rental Payment" },
                          new ModulePageViewModel{AspRouteContainer = "manageOtherPaymentsContainer", PageName = "Search Other Payments" },
                          new ModulePageViewModel{AspRouteContainer = "addUpdateOtherPaymentContainer", PageName = "Add Other Payment" },
                          new ModulePageViewModel{AspRouteContainer = "manageUploadPaymentsContainer", PageName = "Search Payment Uploads" },
                          new ModulePageViewModel{AspRouteContainer = "addUpdateUploadPaymentContainer", PageName = "Upload Payments" },
                    }

                 },
                    new ModuleViewModel
                {
                    isAdmin = true,
                    ModuleName = "Administration",
                    Icon = "fas fa-wrench",
                    CardColor = "bg-danger",
                    TextColor = "text-white",
                    Pages = new List<ModulePageViewModel>
                    {
                         new ModulePageViewModel{AspRouteContainer = null, PageName = "Search Upload Templates" },
                         new ModulePageViewModel{AspRouteContainer = "addUpdateTemplateContainer", PageName = "Add  Upload Template" },
                    }
                },
                new ModuleViewModel
                {
                    isAdmin = true,
                    ModuleName = "Surety",
                    Icon = "fas fa-hand-holding-usd",
                    CardColor = "bg-secondary",
                    TextColor = "text-white",
                    Pages = new List<ModulePageViewModel>
                    {
                         new ModulePageViewModel{AspRouteContainer = null, PageName = "Search Surety" },
                         new ModulePageViewModel{AspRouteContainer = "addUpdateSuretyContainer", PageName = "Add  Surety" },
                    }
                },
           }
        };

        public MenuViewModel menu => Menu;

    }
  
}
