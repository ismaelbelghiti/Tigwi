using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tigwi.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            this.ViewBag.CurrentUser = "Zak";
            return this.View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
