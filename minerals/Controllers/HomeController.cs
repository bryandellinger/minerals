using Microsoft.AspNetCore.Mvc;
using Minerals.ViewModels;

namespace Minerals.Controllers
{
    public class HomeController : Controller
    {
        private MenuDataViewModel data;
        public HomeController(MenuDataViewModel d) => data = d;
        public IActionResult Index() => View(data.menu.Modules);
    }
}