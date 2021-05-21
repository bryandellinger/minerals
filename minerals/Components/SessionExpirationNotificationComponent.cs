using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Components
{
    public class SessionExpirationNotificationComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
