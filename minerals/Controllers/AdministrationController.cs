using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minerals.ViewModels;

namespace Minerals.Controllers
{
    public class AdministrationController : Controller
    {
        private MenuDataViewModel data;
        public AdministrationController(MenuDataViewModel d) => data = d;
        public IActionResult Index() => View(data.menu.Modules.First(x => x.ModuleName == "Administration").Pages);
    }
}