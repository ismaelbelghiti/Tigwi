namespace Tigwi.UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net.NetworkInformation;
    using System.Security.Authentication;
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
            : this(MakeStorage("sefero", "GU0GjvcPoXKzDFgBSPFbWmCPQrIRHAT6fholbMnxtteY5vQVgYTcWKk/25i/F4m9MFoGHXNf4oYgeAKo+mFO5Q=="))
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

        protected void ResetCurrentAccount()
        {
            var cookie = new HttpCookie(AccountCookie, null) { Expires = new System.DateTime(1999, 10, 12) };
            this.Response.SetCookie(cookie);
            this.currentAccount = null;
        }

        /// <summary>
        /// Gets or sets the current account
        /// </summary>
        public IAccountModel CurrentAccount
        {
            get
            {
                return this.currentAccount;
            }

            protected set
            {
                var cookie = new HttpCookie(AccountCookie, null) { Expires = DateTime.MaxValue, HttpOnly = true };
                if (value != null)
                {
                    if (this.CurrentUser.Accounts.Contains(value))
                    {
                        cookie.Value = value.Id.ToString();
                        this.currentAccount = value;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                this.HttpContext.Response.SetCookie(cookie);
            }
        }

        /// <summary>
        /// Gets the current (logged in) user.
        /// </summary>
        public IUserModel CurrentUser
        {
            get
            {
                return this.currentUser;
            }
        }

        protected IUserModel AuthenticateUser(IUserModel value, bool rememberMe)
        {
            Contract.Assert(value != null);

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
            var cookie = new HttpCookie(AccountCookie) { Expires = new DateTime(1999, 10, 12) };
            this.Response.Cookies.Remove(AccountCookie);
            this.Response.Cookies.Add(cookie);
            this.currentUser = null;
            this.currentAccount = null;
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

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var identity = this.User.Identity;
            if (!identity.IsAuthenticated)
            {
                return;
            }
            try
            {
                var user = this.Storage.Users.Find(identity.Name);
                IAccountModel account = null;

                var cookie = this.Request.Cookies[AccountCookie];
                if (cookie != null)
                {
                    Guid accountId;
                    if (Guid.TryParse(cookie.Value, out accountId))
                    {
                        try
                        {
                            account = this.Storage.Accounts.Find(accountId);
                        }
                        catch (AccountNotFoundException)
                        {
                            cookie.Expires = new System.DateTime(1999, 10, 12);
                            this.Response.SetCookie(cookie);
                        }
                    }
                    else
                    {
                        // Reset account cookie
                        cookie.Expires = new System.DateTime(1999, 10, 12);
                        this.Response.SetCookie(cookie);
                    }
                }

                if (account == null)
                {
                    try
                    {
                        account = this.Storage.Accounts.Find(identity.Name);
                    }
                    catch (AccountNotFoundException)
                    {
                    }
                }

                this.currentUser = user;
                this.currentAccount = account;
            }
            catch (UserNotFoundException)
            {
                this.Deauthenticate();
                filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
            }

            if (this.currentUser == null || this.currentAccount == null)
            {
                this.Deauthenticate();
                filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
            }
        }
    }
}