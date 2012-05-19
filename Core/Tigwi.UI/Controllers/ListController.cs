using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tigwi.UI.Controllers
{
    using System.Net;

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
        /// Actually creates or edit the list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EditListViewModel editList, int edit)
        {
            // TODO: This is NOT correct. There should be *TWO* distinct methods Edit and Create.
            IListModel list = null;
            list = edit == 0
                       ? this.Storage.Lists.Create(
                           this.CurrentAccount, editList.ListName, editList.ListDescription, !editList.ListPublic)
                       : this.Storage.Lists.Find(editList.ListId);
            try
            {
                list.Name = editList.ListName;
                list.Description = editList.ListDescription;
                list.IsPrivate = !editList.ListPublic;
                list.Members.Clear();
                foreach (var member in editList.AccountIds)
                {
                    IAccountModel account = this.Storage.Accounts.Find(member);
                    list.Members.Add(account);
                }
                this.Storage.SaveChanges();
                return this.RedirectToAction("Index", "Home");
            }
            catch (Models.Storage.AccountNotFoundException ex)
            {
                if (edit == 0)
                    this.Storage.Lists.Delete(list);
                throw new HttpException((int)HttpStatusCode.NotFound, ex.Message);
            }
            catch (NullReferenceException)
            {
                // TODO: WTFF ?????????????????????????
                if (edit == 0)
                    this.Storage.Lists.Delete(list);
                throw new HttpException((int)HttpStatusCode.NotFound, "The list is empty, it has been deleted.");
            }
            catch (Tigwi.Storage.Library.IsPersonnalList ex)
            {
                return this.RedirectToAction("Index", "Home", new { error = ex.Message });
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
            var list = this.Storage.Lists.Find(id);
            CurrentAccount.AllFollowedLists.Add(list);
            this.Storage.SaveChanges();
            return Json(new {Name=list.Name});
        }

        /// <summary>
        /// Stops the active account from following list id
        /// Idempotent.
        /// </summary>
        /// <returns>The resulting view.</returns>
        [HttpPost]
        public ActionResult UnfollowList(Guid id)
        {
            var list = this.Storage.Lists.Find(id);
            CurrentAccount.AllFollowedLists.Remove(list);
            this.Storage.SaveChanges();
            return Json(new { Name = list.Name });
        }

        /// <summary>
        /// delete a list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteList(Guid id)
        {
            //TODO check whether or not it all went according to plan ...
            //TODO prevent other accounts from deleting your public lists ...
            try
            {
                this.Storage.Lists.Delete(this.Storage.Lists.Find(id));
                return this.RedirectToAction("Index", "Home");
            }
            catch (Tigwi.Storage.Library.IsPersonnalList ex)
            {
                return this.RedirectToAction("Index", "Home", new { error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetList(Guid listId)
        {
            IListModel list = CurrentAccount.AllFollowedLists.Where(l => l.Id == listId).First();
            return Json(new { Name = list.Name,Descr = list.Description,Public = !list.IsPrivate, Members=list.Members.Select(account=>account.Name)});
            
        }

        public ActionResult AddAccount()
        {
            throw new NotImplementedException("ListController.AddAccounts");
        }

        public ActionResult RemoveAccount()
        {
            throw new NotImplementedException("ListController.RemoveAccounts");
        }
    }
}
