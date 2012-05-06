using System.Collections.Generic;

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

        private IAccountModel currentAccount;

        private IUserModel currentUser;

        private IStorageContext storage;

        #endregion

        #region Constructors and Destructors

        public HomeController()
            // : this(new StorageContext(new Storage("__AZURE_STORAGE_ACCOUNT_NAME", "__AZURE_STORAGE_ACCOUNT_KEY")))
        {
        }

        public HomeController(IStorageContext storageContext)
        {
            this.storage = storageContext;
        }

        #endregion

        #region Properties

        public IAccountModel CurrentAccount
        {
            get
            {
                if (this.currentAccount == null)
                {
                    var identity = this.User.Identity;
                    if (this.User != null && identity is CustomIdentity)
                    {
                        this.currentAccount = this.Storage.Accounts.Find((identity as CustomIdentity).AccountId);
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

            protected set
            {
                /*if (!this.CurrentUser.Accounts.Contains(value))
                {
                    throw new NotImplementedException("User not a member of account");
                }*/

                this.currentAccount = value;
            }
        }

        public IUserModel CurrentUser
        {
            get
            {
                var identity = this.User.Identity;
                if (this.currentUser == null && identity is CustomIdentity) 
                {
                    this.currentUser = this.Storage.Users.Find((identity as CustomIdentity).UserId);
                }

                return this.currentUser;
            }

            protected set
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
                        this.Session["Storage"] = new MockStorage();
                    }

                    this.storage = new StorageContext(this.Session["Storage"] as IStorage);
                }

                return this.storage;
            }
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index()
        {
            return this.User.Identity.IsAuthenticated ? this.View(this.Storage.Accounts.Find(this.CurrentAccount.Name)) : this.View();
        }

        #endregion

        #region Methods

        protected void SaveIdentity(bool isPersistent)
        {
            // TODO: refactor everything, no BinaryFormatter, etc.
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