#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
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
    using System.Net;

    using Tigwi.Storage.Library;
    using Tigwi.UI.Models.Storage;
    using Tigwi.UI.Models.Account;

    public class AccountController : HomeController
    {
        public AccountController(IStorage storage)
            : base(storage)
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
        [Authorize]
        public ActionResult List()
        {
            return this.View(CurrentUser);
/*            if (this.CurrentUser != null)
            {
                return this.View(CurrentUser);
            }

            // User must be connected
            throw new NotImplementedException();*/
        }

        /// <summary>
        ///  Checks Whether the account <paramref name="account"/> exists
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AccountExists(string account)
        {
            bool exists = this.Storage.Accounts.Exists(account);
            return Json(new { exists = exists });
        }

        /// <summary>
        /// Shows a page listing all the posts of the account.
        /// </summary>
        /// <returns></returns>
        public ActionResult Show(string accountName)
        {
            IAccountModel account;
            //TODO : catch this 
            if (string.IsNullOrEmpty(accountName) || !this.Storage.Accounts.TryFind(accountName, out account))
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Account was not found");
            }

            return this.View("Show", account);
        }

        public ActionResult Search(string searchString)
        {
            // TODO
            return this.Show(searchString);
        }

        /// <summary>
        /// Makes the given account active (the one which will post things by default, etc.)
        /// </summary>
        /// <returns></returns>
        [Authorize]
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
        public ActionResult Create(AccountCreationViewModel accountCreation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newAccount = this.Storage.Accounts.Create(CurrentUser, accountCreation.Name, accountCreation.Description);
                    this.CurrentAccount = newAccount;

                    IListModel list = this.Storage.Lists.Create(newAccount, newAccount.Name, "Personal list of " + newAccount.Name, false);
                    list.Members.Add(newAccount);
                    this.Storage.SaveChanges();
                    return this.RedirectToAction("Create", accountCreation);
                }
                catch (DuplicateAccountException ex)
                {
                    throw new HttpException(404, ex.Message);
                }
            }

            return this.PartialView("_CreateAccountModal", accountCreation);
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

        /// <summary>
        /// Remove the current user from account
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Remove(Guid id)
        {
            try
            {
                IAccountModel account = this.Storage.Accounts.Find(id);
                account.Users.Remove(CurrentUser);
                this.Storage.SaveChanges();
                if (id == CurrentAccount.Id)
                {
                    CurrentAccount = CurrentUser.Accounts.ElementAt(0);
                }
                return this.RedirectToAction("List", "Account");
            }
            catch (AccountNotFoundException)
            {
                return this.RedirectToAction("Index", "Home", new { error = "This account doesn't exist" });
            }
            catch
            {
                throw new NotImplementedException("Account.Delete");
            }
            //TODO : Catch the good exceptions of Remove 
        }

        [HttpPost]
        public ActionResult IsFollowed(Guid listId)
        {
            bool followed;
            try
            {
                followed = CurrentAccount.PublicFollowedLists.Select(list => list.Id).Contains(listId);
            }
            catch
            {
                followed = false;
            }
            return Json(new {Followed = followed});
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
        public ActionResult Edit(AccountEditViewModel editAccount)
        {
            if (ModelState.IsValid)
            {
                IAccountModel account = this.Storage.Accounts.Find(editAccount.AccountId);
                account.Description = editAccount.Description;

                foreach (var member in editAccount.UserIds)
                {
                    IUserModel user = this.Storage.Users.Find(member);
                    account.Users.Add(user);
                }

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
            try
            {
                IAccountModel account = this.Storage.Accounts.Find(id);
                account.PersonalList.Followers.Add(CurrentAccount);
                return this.View(account);
            }
            catch(AccountNotFoundException)
            {
                return this.RedirectToAction("Index", "Home", new { error = "This account doesn't exist anymore!" });
            }
            catch
            {
                return this.RedirectToAction("Index", "Home", new { error = "Something went wrong!" });
            }
        }

        /// <summary>
        /// Makes the active account follow the given account.
        /// Idempotent.
        /// </summary>
        /// <returns>The resulting view.</returns>
        [HttpPost]
        public ActionResult Follow(Guid id)
        {
            try
            {
                IAccountModel account = this.Storage.Accounts.Find(id);
                
                //Todo redirect to a dedicated view

                IListModel list = this.Storage.Lists.Create(CurrentAccount, account.Name, "My friend " + account.Name, false);
                list.Members.Add(account);
                this.Storage.SaveChanges();

                return Json(new { ok=true });
            }
            catch (AccountNotFoundException ex)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, ex.Message);
            }
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
        /// Shows all the people that follow a list owned by the current account.
        /// </summary>
        /// <returns>The resulting view.</returns>
        public ActionResult Followers()
        {
            return this.View();           
        }

        [HttpPost]
        public ActionResult GetAccount(Guid accountId)
        {
            var account = this.Storage.Accounts.Find(accountId);
            return Json(new { Descr = account.Description, Name = account.Name, Users = account.Users.Select(user => user.Login) });
        }

        [HttpPost]
        public ActionResult AutoComplete(string partialAccountName)
        {
            return Json(this.RawStorage.Account.Autocompletion(partialAccountName, 10));
        }
    }
}
