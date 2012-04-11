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
            throw new NotImplementedException("HomeController.Index");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
