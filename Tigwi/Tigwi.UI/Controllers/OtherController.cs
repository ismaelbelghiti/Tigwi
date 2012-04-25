using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tigwi.UI.Models;

namespace Tigwi.UI.Controllers
{
    public class OtherController : Controller
    {
        //
        // GET: /Other/

        public ActionResult Index(string user="Me")
        {

            return View(new OtherViewModel(user));
        }

    }
}
