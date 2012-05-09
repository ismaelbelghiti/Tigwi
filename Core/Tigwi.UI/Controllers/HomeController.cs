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

    using Tigwi.Storage.Library;

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
           : this(MakeStorage("__AZURE_STORAGE_ACCOUNT_NAME", "__AZURE_STORAGE_ACCOUNT_KEY"))
        {
        }

        public HomeController(IStorageContext storageContext)
        {
            this.storage = storageContext;
        }

        #endregion

        private static IStorageContext MakeStorage(string accountName, string accountKey)
        {
            try
            {
                return new StorageContext(new Storage(accountName, accountKey));
            }
            catch (FormatException)
            {
            }

            return null;
        }

        #region Properties

        private const string AccountCookie = "CURACCOUNT";

        public IAccountModel CurrentAccount
        {
            get
            {
                var user = this.CurrentUser;

                if (user == null)
                {
                    this.currentAccount = null;
                    return null;
                }

                if (this.currentAccount == null)
                {
                    var cookie = this.HttpContext.Request.Cookies[AccountCookie];
                    if (cookie == null)
                    {
                        return null;
                    }

                    Guid accountId;

                    if (Guid.TryParse(cookie.Value, out accountId))
                    {
                        this.currentAccount = this.Storage.Accounts.Find(accountId);
                        if (!user.Accounts.Contains(this.currentAccount))
                        {
                            // TODO: log
                            Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Bad account."));
                            cookie.Expires = DateTime.MinValue;
                            this.Response.SetCookie(cookie);

                            // TODO: this *must* be something that CAN'T fail.
                            this.currentAccount = this.Storage.Accounts.Find(user.Login);
                        }
                    }
                    else
                    {
                        this.currentAccount = this.Storage.Accounts.Find(user.Login);
                    }
                }

                return this.currentAccount;
            }

            protected set
            {
                if (!this.CurrentUser.Accounts.Contains(value))
                {
                    throw new NotImplementedException("User not a member of account");
                }

                var cookie = new HttpCookie(AccountCookie, value.Id.ToString());
                this.HttpContext.Response.SetCookie(cookie);

                this.currentAccount = value;
            }
        }

        public IUserModel CurrentUser
        {
            get
            {
                if (this.currentUser == null)
                {
                    // TODO: store ID.
                    var identity = this.User.Identity;
                    this.currentUser = this.Storage.Users.Find(identity.Name);
                }

                return this.currentUser;
            }
        }

        protected IUserModel AuthenticateUser(IUserModel value, bool rememberMe)
        {
            // Update authentication cookie
            var existingCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
            var version = 1;

            if (existingCookie != null)
            {
                try
                {
                    var existingTicket = FormsAuthentication.Decrypt(existingCookie.Value);
                    version = existingTicket.Version + 1;
                }
                catch (ArgumentException)
                {
                }
            }

            // Reset account cookie
            var cookie = new HttpCookie(AccountCookie, value.Id.ToString());
            this.HttpContext.Response.SetCookie(cookie);

            var ticket = new FormsAuthenticationTicket(
                version, value.Login, DateTime.Now, DateTime.Now + FormsAuthentication.Timeout, rememberMe, value.Id.ToString());
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
                {
                    HttpOnly = true,
                    Expires = DateTime.Now + FormsAuthentication.Timeout,
                    Path = FormsAuthentication.FormsCookiePath,
                    Domain = FormsAuthentication.CookieDomain,
                    Secure = FormsAuthentication.RequireSSL
                };
            this.HttpContext.Response.SetCookie(authCookie);

            this.currentUser = value;
            this.currentAccount = null;

            return value;
        }

        protected void Deauthenticate()
        {
            FormsAuthentication.SignOut();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName) { Expires = DateTime.MinValue };
            this.Response.AppendCookie(cookie);
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

        [Obsolete]
        protected void SaveIdentity(bool isPersistent)
        {
        }

        #endregion
    }
}