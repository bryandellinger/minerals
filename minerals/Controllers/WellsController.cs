using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minerals.ViewModels;

namespace Minerals.Controllers
{
    public class WellsController : Controller
    {
        private MenuDataViewModel data;
        public WellsController(MenuDataViewModel d) => data = d;
        public IActionResult Index() => View(data.menu.Modules.First(x => x.ModuleName == "Wells").Pages);
    }
}