namespace Tigwi.UI.Controllers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using Tigwi.Storage.Library;
    using Tigwi.UI.Models;
    using Tigwi.UI.Models.Storage;

    public class HomeController : Controller
    {
        #region Constants and Fields

        private const string AccountCookie = "CURACCOUNT";

        private IAccountModel currentAccount;

        private IStorage rawStorage;

        private IStorageContext storage;

        #endregion

        #region Constructors and Destructors

        public HomeController()
            : this(MakeStorage("__AZURE_STORAGE_ACCOUNT_NAME", "__AZURE_STORAGE_ACCOUNT_KEY"))
        {
        }

        public HomeController(IStorage storage)
        {
            if (storage == null)
            {
                return;
            }

            this.rawStorage = storage;
            this.storage = new StorageContext(storage);
        }

        public HomeController(IStorageContext storageContext)
        {
            this.storage = storageContext;
        }

        #endregion

        #region Public Properties

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
                if (!this.TrySetCurrentAccount(value))
                {
                    // TODO: Account not valid
                    throw new NotImplementedException("Account not valid.");
                }
            }
        }

        /// <summary>
        /// Gets the current (logged in) user.
        /// </summary>
        public IUserModel CurrentUser { get; private set; }

        public string Error { get; set; }

        #endregion

        #region Properties

        protected IStorage RawStorage
        {
            get
            {
                // Berk
                if (this.rawStorage == null)
                {
                    if (this.Session["Storage"] == null)
                    {
                        this.Session["Storage"] = new MockStorage();
                    }

                    var storageContext = new StorageContext(this.Session["Storage"] as IStorage);
                    this.rawStorage = storageContext.StorageObj;
                    this.storage = storageContext;
                }

                return this.rawStorage;
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

                    var storageContext = new StorageContext(this.Session["Storage"] as IStorage);
                    this.rawStorage = storageContext.StorageObj;
                    this.storage = storageContext;
                }

                return this.storage;
            }
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index(string error)
        {
            ViewBag.error = error;
            return this.User.Identity.IsAuthenticated
                       ? this.View(this.Storage.Accounts.Find(this.CurrentAccount.Name))
                       : this.View();
        }

        #endregion

        #region Methods

        protected IUserModel AuthenticateUser(IUserModel value, bool rememberMe)
        {
            Contract.Assert(value != null);

            var ticket = new FormsAuthenticationTicket(
                1, 
                value.Login, 
                DateTime.Now, 
                DateTime.Now + FormsAuthentication.Timeout, 
                rememberMe, 
                value.Id.ToString());
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
                {
                    HttpOnly = true, 
                    Expires = DateTime.Now + FormsAuthentication.Timeout, 
                    Path = FormsAuthentication.FormsCookiePath, 
                    Domain = FormsAuthentication.CookieDomain, 
                    Secure = FormsAuthentication.RequireSSL
                };
            this.HttpContext.Response.SetCookie(authCookie);

            this.CurrentUser = value;
            this.ResetCurrentAccount();

            return value;
        }

        protected void Deauthenticate()
        {
            FormsAuthentication.SignOut();
            this.CurrentUser = null;
            this.ResetCurrentAccount();
        }

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
                            this.ResetCurrentAccount();
                        }
                    }
                    else
                    {
                        this.ResetCurrentAccount();
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

                this.CurrentUser = user;
                this.currentAccount = account;
            }
            catch (UserNotFoundException)
            {
                this.Deauthenticate();
                filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
            }

            if (this.CurrentUser != null && this.currentAccount != null)
            {
                return;
            }

            this.Deauthenticate();
            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }

        protected void ResetCurrentAccount()
        {
            // Hack because DateTime.MinValue means "session cookie". Microsoft logic!
            var cookie = new HttpCookie(AccountCookie, null) { Expires = DateTime.MinValue.AddMilliseconds(1) };
            this.Response.SetCookie(cookie);
            this.currentAccount = null;
        }

        protected bool TrySetCurrentAccount(IAccountModel value)
        {
            if (this.CurrentUser.Accounts.Contains(value))
            {
                var cookie = new HttpCookie(AccountCookie, value.Id.ToString()) { HttpOnly = true };
                this.Response.SetCookie(cookie);
                this.currentAccount = value;
                return true;
            }

            return false;
        }

        private static IStorage MakeStorage(string accountName, string accountKey)
        {
            try
            {
                return new Storage(accountName, accountKey);
            }
            catch (FormatException)
            {
            }

            return null;
        }

        #endregion
    }
}