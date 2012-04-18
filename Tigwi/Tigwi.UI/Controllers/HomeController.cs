using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tigwi.UI.Controllers
{
    using Tigwi.UI.Models;

    public class HomeController : Controller
    {
        public HomeController()
            : this(new StorageContext(null))
        {
        }

        public HomeController(IStorageContext storageContext)
        {
            this.storage = storageContext;

            // Load session variables
            var user = (Guid)this.Session["CurrentUser"];
            var account = (Guid)this.Session["CurrentAccount"];
            this.currentUser = this.Storage.Users.Find(user);
            this.currentAccount = this.Storage.Accounts.Find(account);
        }

        private readonly IStorageContext storage;

        protected IStorageContext Storage
        {
            get
            {
                return this.storage;
            }
        }

        private AccountModel currentAccount;

        protected AccountModel CurrentAccount
        {
            get
            {
                return this.currentAccount;
            }

            set
            {
                if (!this.CurrentUser.Accounts.Contains(value))
                {
                    throw new NotImplementedException();
                }

                // Store the current account in the session
                this.Session["CurrentAccount"] = value.Id;
                this.currentAccount = value;
            }
        }

        private readonly UserModel currentUser;

        protected UserModel CurrentUser
        {
            get
            {
                return this.currentUser;
            }
        }

        public ActionResult Index()
        {
            ViewBag.CurrentUser = "Zak";
            return this.View();
        }

        public ActionResult Xoxo()
        {
            ViewBag.ModelValid = true;
            return this.View(new AccountModel());
        }

        [HttpPost]
        public ActionResult Xoxo(Guid id, string name, string description)
        {
            AccountModel account = null; // new AccountModel { Id = id, Name = name, Description = description };
            ViewBag.ModelValid = ModelState.IsValid;
            return this.View(account);
        }

        protected bool CheckForConnection()
        {
            throw new NotImplementedException("HomeController.CheckForConnection");
        }
    }
}
