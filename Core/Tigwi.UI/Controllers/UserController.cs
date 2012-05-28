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
        public UserController(IStorage storage)
            : base(storage)
        {
        }

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
                    var auth = new Tigwi.Auth.PasswordAuth(RawStorage, userLogOnViewModel.Login, userLogOnViewModel.Password);
                    Guid userId = auth.Authenticate();
                    var loggingUser = this.Storage.Users.Find(userLogOnViewModel.Login);
                    this.AuthenticateUser(loggingUser, userLogOnViewModel.RememberMe);

                    return this.RedirectToAction("Index", "Home");
                    // return this.RedirectToAction("Timeline", "Account");
                }
                catch (Tigwi.Auth.AuthFailedException)
                {
                    ModelState.AddModelError("Login", "Bad login/password");
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
            this.Deauthenticate();

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
                    var hashedPass = Auth.PasswordAuth.HashPassword(registerViewModel.Password);
                    
                    // IUserRepository::Create creates a main account
                    var newUser = this.Storage.Users.Create(registerViewModel.Login, registerViewModel.Email, hashedPass);
                    this.AuthenticateUser(newUser, registerViewModel.RememberMe);
                    this.CurrentAccount = newUser.MainAccount;
                    IListModel list = this.Storage.Lists.Create(CurrentAccount, CurrentAccount.Name, "Personal list of " + CurrentAccount.Name, false);
                    list.Members.Add(CurrentAccount);
                    this.Storage.SaveChanges();
                    return this.RedirectToAction("Index", "Home");
                }
                catch (DuplicateUserException ex)
                {
                    // TODO: We need more granularity (login failed ? email failed ? propositions ?)
                    ModelState.AddModelError("Login", ex.Message);
                }
                catch (DuplicateAccountException ex)
                {
                    ModelState.AddModelError("Login", ex.Message);
                }
            }

            // Somthing went wrong, display register page again
            return this.View(registerViewModel);
        }

        [HttpPost]
        public ActionResult ValidateLogin(string login)
        {
            return Json(this.Storage.Users.Exists(login));
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

        [Authorize]
        public ActionResult ListApiKeys()
        {
            return this.View(CurrentUser.ApiKeys);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GenerateApiKey(GenerateApiKeyViewModel model)
        {
            if(ModelState.IsValid)
            {
                this.CurrentUser.GenerateApiKey(model.ApplicationName);
            }

            return this.RedirectToAction("ListApiKeys","User");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeactivateApiKey(DeactivateApiKeyViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.CurrentUser.DeactivateApiKey(model.ApiKey);
            }

            return this.RedirectToAction("ListApiKeys","User");
        }

        /// <summary>
        ///  Checks whether the user <paramref name="user"/> exists
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserExists(string user)
        {
            bool exists = this.Storage.Users.Exists(user);
            return Json(new { exists = exists });
        }
    }
}
