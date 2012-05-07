using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tigwi.UI.Controllers
{
    using System.Web.Security;

    using Tigwi.Storage.Library;

    using Tigwi.UI.Models;
    using Tigwi.UI.Models.Storage;
    using Tigwi.UI.Models.User;

    public class UserController : HomeController
    {
        public UserController()
        {}

        public UserController(StorageContext storage)
            : base(storage)
        {}

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
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: real authentication
                    var loggingUser = this.Storage.Users.Find(userLogOnViewModel.Login);
                    this.CurrentUser = loggingUser;

                    this.SaveIdentity(userLogOnViewModel.RememberMe);

                    return this.RedirectToAction("Index", "Home");
                    // return this.RedirectToAction("Timeline", "Account");
                }
                catch (UserNotFoundException ex)
                {
                    ModelState.AddModelError("Login", ex.Message);
                }
            }

            return this.View(userLogOnViewModel);
        }

        public ActionResult LogOut()
        {
            this.CurrentUser = null;
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName) { Expires = DateTime.MinValue };
            this.Response.AppendCookie(cookie);

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Shows a page proposing the user to register.
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return this.View();
        }

        /// <summary>
        /// Register a new user into the system.
        /// </summary>
        /// <param name="registerViewModel">A ViewModel containing the data useful for the user creation.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: real authentication
                    var newUser = this.Storage.Users.Create(registerViewModel.Login, registerViewModel.Email);

                    try
                    {
                        var newAccount = this.Storage.Accounts.Create(newUser, registerViewModel.Login, string.Empty);
              
                        this.CurrentUser = newUser;
                        this.CurrentAccount = newAccount;
                        this.Storage.SaveChanges();
                        this.SaveIdentity(registerViewModel.RememberMe);
                        return this.RedirectToAction("Index", "Home");
                    }
                    catch (DuplicateAccountException ex)
                    {
                        this.Storage.Users.Delete(newUser);
                        this.Storage.SaveChanges();
                        ModelState.AddModelError("Login", ex.Message);
                    }
                }
                catch (DuplicateUserException ex)
                {
                    // TODO: We need more granularity (login failed ? email failed ? propositions ?)
                    ModelState.AddModelError("Login", ex.Message);
                }
            }

            // Somthing went wrong, display register page again
            return this.View(registerViewModel);
        }

        public ActionResult Welcome()
        {
            throw new NotImplementedException("UserController.Welcome");
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
