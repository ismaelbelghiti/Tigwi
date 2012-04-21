using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tigwi.UI.Controllers
{
    using System.Web.Security;

    using Tigwi.UI.Models;

    public class UserController : HomeController
    {
        /// <summary>
        /// Show a page proposing the user to log in.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOn()
        {
            return this.View();
        }

        /// <summary>
        /// If credentials are correct, logs the given user in.
        /// Otherwise, redirect to the LogOn page with an error message.
        /// </summary>
        /// <param name="userLogOnViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOn(UserLogOnViewModel userLogOnViewModel)
        {
            // TODO: implement user authentication :-)
            if (ModelState.IsValid)
            {
                this.Storage.Users.Create("Elarnon", "k@o.n");
                var loggingUser = this.Storage.Users.Find(userLogOnViewModel.Login);
                this.CurrentUser = loggingUser;

                return this.View("Error");
            }

            throw new NotImplementedException("UserController.LogOn[POST]");
        }

        /// <summary>
        /// Shows a page proposing the user to register.
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            throw new NotImplementedException("UserController.Register");
        }

        /// <summary>
        /// Register a new user into the system.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            throw new NotImplementedException("UserController.Register[POST]");
        }

        /// <summary>
        /// Shows a page asking for deactivation of the given user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Deactivate(int id)
        {
            throw new NotImplementedException("UserController.Deactivate");
        }

        /// <summary>
        /// Deactivates the given account (consider it an immediate deletion, because we don't have any
        /// mechanism in the storage to handle "true" deactivation).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Deactivate(int id, FormCollection collection)
        {
            throw new NotImplementedException("UserController.Deactivate[POST]");
        }
    }
}
