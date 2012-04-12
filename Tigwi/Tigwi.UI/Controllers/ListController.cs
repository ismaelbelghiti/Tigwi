using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tigwi.UI.Controllers
{
    public class ListController : Controller
    {
        /// <summary>
        /// Shows the messages posted in the list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public ActionResult Details(string listName)
        {
            throw new NotImplementedException("ListController.Details");
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
        /// <param name="listCreateViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(/*ListCreateViewModel*/object listCreateViewModel)
        {
            throw new NotImplementedException("ListController.Create[POST]");
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
