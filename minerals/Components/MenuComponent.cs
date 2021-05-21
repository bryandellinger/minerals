using Microsoft.AspNetCore.Mvc;
using Minerals.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Components
{
    public class MenuComponent : ViewComponent
    {
        private MenuDataViewModel data;
        public MenuComponent(MenuDataViewModel d) => data = d;
        public IViewComponentResult Invoke() => View(data);
    }
}
