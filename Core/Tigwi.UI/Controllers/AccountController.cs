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
    using Tigwi.UI.Models.Account;

    public class AccountController : HomeController
    {
        public AccountController()
        {
        }

        public AccountController(IStorageContext storageContext)
            : base(storageContext)
        {
        }
        
        [Authorize]
        public ActionResult Timeline()
        {
            if (this.CurrentAccount == null)
            {
                throw new NotImplementedException("Not connected.");
            }

            return this.View();
        }

        /// <summary>
        /// Shows a page listing all the accounts of the active user.
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            if (this.CurrentUser != null)
            {
                return this.View(CurrentUser);
            }

            // User must be connected
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Checks Whether the account <paramref name="account"/> exists
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AccountExists(string account)
        {
            //TODO check whether account exists or not <3
            return Json(new { exists = true });
        }

        /// <summary>
        /// Shows a page listing all the posts of the user.
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult ShowAccount(SearchViewModel search)
        {
            try
            {
                return this.View(this.Storage.Accounts.Find(search.searchString));
            }
            catch(AccountNotFoundException ex)
            {
                return this.RedirectToAction("Index", "Home", new { error = ex.Message});
            }
        }


        /// <summary>
        /// Makes the given account active (the one which will post things by default, etc.)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [ValidateInput(false)]
        public ActionResult MakeActive(string accountName)
        {
            try
            {
                var account = this.Storage.Accounts.Find(accountName);

                try
                {
                    this.CurrentAccount = account;
                    return this.RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (AccountNotFoundException ex)
            {
                return this.RedirectToAction("Index", "Home", new { error = ex.Message });
            }
        }

        /// <summary>
        /// Shows a form for creating a new account associated with the active user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return this.RedirectToAction("Index","Home");
        }

        /// <summary>
        /// Creates a new account associated with the active user (i.e. the active user will
        /// be the administrator and sole member of the account).
        /// </summary>
        /// <param name="accountCreation"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(AccountCreationViewModel accountCreation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newAccount = this.Storage.Accounts.Create(CurrentUser, accountCreation.Name, accountCreation.Description);
                    this.CurrentAccount = newAccount;
                    //TODO
                    this.Storage.SaveChanges();
                    return this.RedirectToAction("Create", accountCreation);
                }
                catch (DuplicateAccountException ex)
                {
                    return this.RedirectToAction("Index", "Home", new { error = ex.Message });
                }
            }
            //something went wrong
            return this.View(accountCreation);
        }


        /// <summary>
        /// Delete the account
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                IAccountModel account = this.Storage.Accounts.Find(id);
                this.Storage.Accounts.Delete(account);
                this.Storage.SaveChanges();
                if (id == CurrentAccount.Id)
                {
                    CurrentAccount = CurrentUser.Accounts.ElementAt(0);
                }
                return this.RedirectToAction("List", "Account");
            }
            catch (AccountNotFoundException)
            {
                return this.RedirectToAction("Index", "Home", new { error = "This account doesn't exist"});
            }
            catch
            {
                throw new NotImplementedException("Account.Delete");
            }
        }


        [HttpPost]
        public ActionResult IsFollowed(Guid listId)
        {
            return Json(new {Followed = CurrentAccount.PublicFollowedLists.Select(list => list.Id).Contains(listId)});
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
        [ValidateInput(false)]
        public ActionResult Edit(AccountEditViewModel editAccount)
        {
            if (ModelState.IsValid)
            {
                IAccountModel account = this.Storage.Accounts.Find(editAccount.AccountId);
                account.Description = editAccount.Description;
                this.Storage.SaveChanges();
                return this.RedirectToAction(editAccount.ReturnAction, editAccount.ReturnController);
            }
            return this.RedirectToAction("Index", "Home", new { error = "Invalid attributes, try again !" });
        }

        /// <summary>
        /// Show all the people an account is following.
        /// </summary>
        /// <returns>The resulting view.</returns>
        public ActionResult Following(Guid id)
        {
            IAccountModel account = this.Storage.Accounts.Find(id);
            account.PersonalList.Followers.Add(CurrentAccount);
            return this.View(account);
        }

        /// <summary>
        /// Makes the active account follow the given account.
        /// Idempotent.
        /// </summary>
        /// <returns>The resulting view.</returns>
        [HttpPost]
        public ActionResult Follow(Guid id)
        {
            IListModel list = CurrentAccount.PersonalList;
            try
            {
                IAccountModel account = this.Storage.Accounts.Find(id);
                list.Members.Add(account);
                this.Storage.SaveChanges();
                return this.RedirectToAction("Index", "Home");
            }
            catch (Tigwi.UI.Models.Storage.AccountNotFoundException ex)
            {
                return this.RedirectToAction("Index", "Home", new { error = ex.Message });
            }
            //Todo redirect to a dedicated view
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

        [HttpPost]
        public ActionResult GetAccount(Guid accountId)
        {
            IAccountModel account = this.Storage.Accounts.Find(accountId);
            return Json(new { Descr = account.Description, Name = account.Name });
        }
    }
}
