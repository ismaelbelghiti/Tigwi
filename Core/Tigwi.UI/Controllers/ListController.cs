using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tigwi.UI.Controllers
{
    using Tigwi.UI.Models;

    public class ListController : HomeController
    {

        /// <summary>
        /// Shows the messages posted in the list.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public ActionResult Details(Guid listId)
        {
            var list = this.Storage.Lists.Find(listId);

            return this.View(list);
        }

        /// <summary>
        /// Show the members of a list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Members(string listName)
        {
            throw new NotImplementedException("ListController.Members");
        }

        /// <summary>
        /// Show an interface for the creation of a new list associated with the active account.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            throw new NotImplementedException("ListController.Create");
        }

        /// <summary>
        /// Actually creates the list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EditListViewModel editList)
        {
            IListModel list = this.Storage.Lists.Create(CurrentAccount, editList.Name, editList.Description, true);
            try
            {
                list.Members.Clear();
                foreach (var member in editList.AccountIds)
                {
                    IAccountModel account = this.Storage.Accounts.Find(member);
                    list.Members.Add(account);
                }
                this.Storage.SaveChanges();
                return this.RedirectToAction("Index", "Home");
            }
            catch (Tigwi.UI.Models.Storage.AccountNotFoundException ex)
            {
                this.Storage.Lists.Delete(list);
                return this.RedirectToAction("Index", "Home", new { error = ex.Message });
            }
            catch (System.NullReferenceException)
            {
                this.Storage.Lists.Delete(list);
                return this.RedirectToAction("Index", "Home", new { error = "The list is empty" });
            }
        }

        /// <summary>
        /// Makes the active account follow the given list.
        /// Idempotent.
        /// </summary>
        /// <returns>The resulting view.</returns>
        [HttpPost]
        public ActionResult FollowList(Guid id)
        {
            CurrentAccount.AllFollowedLists.Add(this.Storage.Lists.Find(id));
            this.Storage.SaveChanges();
            return this.RedirectToAction("Index", "Home");
            //Todo redirect to a dedicated view
        }

        public ActionResult AddAccount()
        {
            throw new NotImplementedException("ListController.AddAccounts");
        }

        public ActionResult RemoveAccount()
        {
            throw new NotImplementedException("ListController.RemoveAccounts");
        }

        /// <summary>
        /// Show an interface for modification of the name/description of the given list, if the active account is its owner.
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            throw new NotImplementedException("ListController.Edit");
        }

        /// <summary>
        /// Actually updates the given list.
        /// </summary>
        /// <param name="listEditViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(/*ListEditViewModel*/object listEditViewModel)
        {
            throw new NotImplementedException("ListController.Edit[POST]");
        }
    }
}
