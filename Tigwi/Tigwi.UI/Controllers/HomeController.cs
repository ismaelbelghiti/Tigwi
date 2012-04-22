namespace Tigwi.UI.Controllers
{
    #region

    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;

    using StorageLibrary;

    using Tigwi.UI.Models.Storage;

    #endregion

    public class HomeController : Controller
    {
        #region Constants and Fields

        private StorageUserModel currentUser;

        private readonly IStorageContext storage;

        private StorageAccountModel currentAccount;

        #endregion

        #region Constructors and Destructors

        public HomeController()
        {
            // this.storage = new StorageContext(new Storage("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="));
            this.storage = new StorageContext(new StorageTmp());
        }

        public HomeController(IStorageContext storageContext)
        {
            this.storage = storageContext;
        }

        #endregion

        #region Properties

        protected StorageAccountModel CurrentAccount
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

                // Authenticate
                var ticket = new FormsAuthenticationTicket(
                    1, this.CurrentUser.Login, DateTime.Now, DateTime.MaxValue, false, this.CurrentUser.Id + "/" + value.Id);
                var encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                this.Response.Cookies.Add(cookie);

                this.currentAccount = value;
            }
        }

        protected StorageUserModel CurrentUser
        {
            get
            {
                return this.currentUser;
            }

            set
            {
                this.currentUser = value;
                this.CurrentAccount = this.Storage.Accounts.Find(value.Login);
            }
        }

        protected IStorageContext Storage
        {
            get
            {
                return this.storage;
            }
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index()
        {
            this.ViewBag.CurrentUser = "Zak";
            return this.View();
        }

        #endregion

        #region Methods

        protected bool CheckForConnection()
        {
            throw new NotImplementedException("HomeController.CheckForConnection");
        }

        #endregion
    }
}