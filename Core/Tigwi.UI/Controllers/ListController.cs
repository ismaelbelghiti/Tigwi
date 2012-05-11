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
        public ActionResult Create(EditListViewModel editList)
        {
            IListModel list = this.Storage.Lists.Create(CurrentAccount, editList.Name, "", true);
            foreach (var member in editList.UserIds)
            {
                IAccountModel account = this.Storage.Accounts.Find(member);
                list.Members.Add(account);
            }
            this.Storage.SaveChanges();
            return this.RedirectToAction("Index", "Home");
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
        public ActionResult Edit(/*ListEditViewModel*/object listEditViewModel)
        {
            throw new NotImplementedException("ListController.Edit[POST]");
        }
    }
}
