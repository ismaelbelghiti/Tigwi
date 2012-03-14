using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Controllers
{
    public class AuthController : Controller
    {
        //
        // GET: /Auth/Login/ ?id=3
        public ActionResult Login()
        {
            ViewData["Test"] = Session["Username"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            Session["Username"] = login;
            ViewData["Test"] = "Foo";
            return View();
        }

        public ActionResult LogOff()
        {
            Session.Remove("Username");
            return RedirectToAction("Login");
        }
    }
}
