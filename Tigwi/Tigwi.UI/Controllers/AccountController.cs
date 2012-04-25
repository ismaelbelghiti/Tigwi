using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Tigwi.UI.Models;

namespace Tigwi.UI.Controllers
{
    using Tigwi.UI.Models.Storage;

    public class AccountController : HomeController
    {
        public AccountController()
        {
        }

        public AccountController(IStorageContext storageContext)
            : base(storageContext)
        {
        }
        
        public ActionResult Timeline()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows a page listing all the accounts of the active user.
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            if (this.CurrentUser != null)
            {
                return this.View(this.CurrentUser.Accounts);
            }

            // User must be connected
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes the given account active (the one which will post things by default, etc.)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MakeActive(Guid accountId)
        {
            var account = this.Storage.Accounts.Find(accountId);

            try
            {
                // Set current account with automatic validation
                this.CurrentAccount = account;

                // Tell the user everything went OK
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                // Tell the user he must be connected
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Shows a form for creating a new account associated with the active user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            throw new NotImplementedException("AccountController.Create");
        }

        /// <summary>
        /// Creates a new account associated with the active user (i.e. the active user will
        /// be the administrator and sole member of the account).
        /// </summary>
        /// <param name="accountCreation"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(/*AccountCreationModel*/object accountCreation)
        {
            throw new NotImplementedException("AccountController.Create[POST]");
        }

        /// <summary>
        /// Show a form to edit the given account.
        /// If no account is given, edit the active account instead.
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            throw new NotImplementedException("AccountController.Edit");
        }

        /// <summary>
        /// Updates the given account's parameters.
        /// </summary>
        /// <param name="accountEdit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(/*AccountEditModel*/object accountEdit)
        {
            throw new NotImplementedException("AccountController.Edit");
        }

        /// <summary>
        /// Show all the people an account is following.
        /// </summary>
        /// <returns>The resulting view.</returns>
        public ActionResult Following()
        {
            throw new NotImplementedException("AccountController.Following");
        }

        /// <summary>
        /// Makes the active account follow the given account.
        /// Idempotent.
        /// </summary>
        /// <returns>The resulting view.</returns>
        public ActionResult Follow()
        {
            throw new NotImplementedException("AccountController.Follow");
        }

        /// <summary>
        /// Makes the active account unfollow the given account.
        /// Idempotent.
        /// </summary>
        /// <returns>The resulting view.</returns>
        public ActionResult UnFollow()
        {
            throw new NotImplementedException("AccountController.UnFollow");
        }

        /// <summary>
        /// Shows all the people that follow a given account.
        /// </summary>
        /// <returns>The resulting view.</returns>
        public ActionResult Followers()
        {
            throw new NotImplementedException("AccountController.Followers");
        }
    }
}
