namespace Tigwi.UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using StorageLibrary;

    using Tigwi.UI.Models;
    using Tigwi.UI.Models.Storage;

    public class HomeController : Controller
    {
        #region Constants and Fields

        private StorageAccountModel currentAccount;

        private StorageUserModel currentUser;

        private IStorageContext storage;

        #endregion

        #region Constructors and Destructors

        public HomeController()
        {
            // this.storage = new StorageContext(new Storage("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="));
            // this.storage = new StorageContext(new StorageTmp());
        }

        public HomeController(IStorageContext storageContext)
        {
            this.storage = storageContext;
        }

        #endregion

        #region Properties

        internal StorageAccountModel CurrentAccount
        {
            get
            {
                if (this.currentAccount == null)
                {
                    CustomIdentity identity;
                    if (this.User != null && (identity = this.User.Identity as CustomIdentity) != null)
                    {
                        this.currentAccount = this.Storage.Accounts.Find(identity.AccountId);
                    }
                    else
                    {
                        var user = this.CurrentUser;
                        if (user != null)
                        {
                            this.currentAccount = this.Storage.Accounts.Find(user.Login);
                        }
                    }
                }

                return this.currentAccount;
            }

            set
            {
                if (!this.CurrentUser.Accounts.Contains(value))
                {
                    throw new NotImplementedException("User not a member of account");
                }

                this.currentAccount = value;
            }
        }

        internal StorageUserModel CurrentUser
        {
            get
            {
                if (this.currentUser == null && this.User.Identity is CustomIdentity)
                {
                    var storageInfos = this.User.Identity as CustomIdentity;
                    if (storageInfos != null)
                    {
                        this.currentUser = this.Storage.Users.Find(storageInfos.UserId);
                    }
                }

                return this.currentUser;
            }

            set
            {
                this.currentUser = value;
                this.currentAccount = null;
            }
        }

        protected IStorageContext Storage
        {
            get
            {
                // Hack because we are using a StorageTmp
                if (this.storage == null)
                {
                    if (this.Session["Storage"] == null)
                    {
                        this.Session["Storage"] = new StorageContext(new StorageTmp());
                    }

                    this.storage = this.Session["Storage"] as IStorageContext;
                }

                return this.storage;
            }
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index()
        {
            this.ViewBag.CurrentUser = "Zak";
            this.ViewBag.Accounts = new List<string>();
            this.ViewBag.Accounts.Add("Me");
            this.ViewBag.Accounts.Add("A");
            this.ViewBag.Accounts.Add("B");
            this.ViewBag.curAccount = "Me";
            return this.View();
        }

        #endregion

        #region Methods

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.ViewBag.CurrentUser = this.CurrentUser;
            this.ViewBag.CurrentAccount = this.CurrentAccount;
        }

        protected void SaveIdentity(bool isPersistent)
        {
            var existingCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
            var version = 1;
            var userData = new CookieData { UserId = this.CurrentUser.Id, AccountId = this.CurrentAccount.Id };

            if (existingCookie != null)
            {
                try
                {
                    version = FormsAuthentication.Decrypt(existingCookie.Value).Version + 1;
                }
                catch (ArgumentException)
                {
                }
            }

            // Serialize data
            var stream = new MemoryStream();
            (new BinaryFormatter()).Serialize(stream, userData);
            var serializedUserData = Convert.ToBase64String(stream.ToArray());

            // Create ticket
            var ticket = new FormsAuthenticationTicket(
                version, 
                this.CurrentUser.Login, 
                DateTime.Now, 
                DateTime.Now + FormsAuthentication.Timeout, 
                isPersistent, 
                serializedUserData);
            var encrypted = FormsAuthentication.Encrypt(ticket);

            // Send cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            this.Response.Cookies.Add(cookie);
        }

        #endregion
    }
}