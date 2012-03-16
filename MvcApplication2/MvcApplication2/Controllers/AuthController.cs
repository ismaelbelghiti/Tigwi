using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class AuthController : Controller
    {
        AuthEntities authDb = new AuthEntities();
        //
        // GET: /Auth/Login/ ?id=3
        public ActionResult Login()
        {
            ViewData["Test"] = authDb.PasswordAuth.Count()+" active accounts. "+Session["Username"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            bool valid=false;
            PasswordAuthModel auth;
            try
            {
                auth = authDb.PasswordAuth.Single(u => u.UserName == login);
                valid = auth.CheckPassword(password);
                if (!valid) throw new Exception();
                Session["Username"] = login;
                Session["UserId"] = auth.UserId;
                ViewData["Test"] = "Foo";
                return RedirectToAction("UserInfo");
            }
            catch (Exception e) {
                ViewData["Test"] = "Mauvais login/mot de passe";
                return View();
            }
        }

        public ActionResult UserInfo()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            Session.Remove("Username");
            Session.Remove("UserId");
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(int userId, string username, string password)
        {
            if (authDb.PasswordAuth.Where(u => u.UserName == username).Count() == 0)
            {
                authDb.PasswordAuth.Add(new PasswordAuthModel { UserId = userId, UserName = username, Password = password });
                authDb.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ViewData["message"] = "Ce nom d'utilisateur est déjà utilisé";
                return View();
            }
        }

        public ActionResult List()
        {
            var users = authDb.PasswordAuth.ToList();
            return View(users);
        }
    }
}
