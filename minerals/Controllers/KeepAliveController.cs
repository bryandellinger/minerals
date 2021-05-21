using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Minerals.Controllers
{
    [AllowAnonymous]
    public class KeepAliveController : Controller
    {
        //
        // GET: /KeepAlive
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Content("I am alive!");
        }
    }
}