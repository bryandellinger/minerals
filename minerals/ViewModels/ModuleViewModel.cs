using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class ModuleViewModel
    {
        public string ModuleName { get; set; }
        public bool isAdmin { get; set; }
        public string Icon { get; set; }
        public string CardColor { get; set; }
        public string TextColor { get; set; }
        public IEnumerable<ModulePageViewModel> Pages { get; set; }
    }
}
