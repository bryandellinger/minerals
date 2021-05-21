using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minerals.ViewModels;

namespace Minerals.Controllers
{
    public class UnitsController : Controller
    {
        private MenuDataViewModel data;
        public UnitsController(MenuDataViewModel d) => data = d;
        public IActionResult Index() => View(data.menu.Modules.First(x => x.ModuleName == "Units").Pages);

    }
}