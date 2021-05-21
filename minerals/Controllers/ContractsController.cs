using Microsoft.AspNetCore.Mvc;
using Minerals.ViewModels;
using System.Linq;

namespace Minerals.Controllers
{
    public class ContractsController : Controller
    {
        private MenuDataViewModel data;
        public ContractsController(MenuDataViewModel d) => data = d;
        public IActionResult Index() => View(data.menu.Modules.First(x => x.ModuleName == "Contracts").Pages);
      
    }
}