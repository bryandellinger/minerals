using Microsoft.AspNetCore.Mvc;
using Minerals.ViewModels;
using System.Linq;

namespace Minerals.Controllers
{
    /// <summary> Account Controller </summary>
    public class AccountingController : Controller
    {
        private MenuDataViewModel data;
        public AccountingController(MenuDataViewModel d) => data = d;
        public IActionResult Index() => View(data.menu.Modules.First(x => x.ModuleName == "Accounting").Pages);
    }
}