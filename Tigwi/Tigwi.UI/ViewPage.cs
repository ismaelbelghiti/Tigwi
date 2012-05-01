namespace Tigwi.UI
{
    using System.Web.Mvc;

    using Tigwi.UI.Controllers;
    using Tigwi.UI.Models.Storage;

    public abstract class ViewPage<TModel> : WebViewPage<TModel>
    {
        #region Constants and Fields

        private StorageAccountModel currentAccount;

        private StorageUserModel currentUser;

        #endregion

        #region Public Properties

        public StorageAccountModel CurrentAccount
        {
            get
            {
                if (this.currentAccount == null)
                {
                    var controller = this.ViewContext.Controller as HomeController;

                    if (controller != null)
                    {
                        this.currentAccount = controller.CurrentAccount;
                    }
                }

                return this.currentAccount;
            }
        }

        public StorageUserModel CurrentUser
        {
            get
            {
                if (this.currentAccount == null)
                {
                    var controller = this.ViewContext.Controller as HomeController;

                    if (controller != null)
                    {
                        this.currentUser = controller.CurrentUser;
                    }
                }

                return this.currentUser;
            }
        }

        #endregion
    }

    public abstract class ViewPage : WebViewPage
    {
        #region Constants and Fields

        private StorageAccountModel currentAccount;

        private StorageUserModel currentUser;

        #endregion

        #region Public Properties

        public StorageAccountModel CurrentAccount
        {
            get
            {
                if (this.currentAccount == null)
                {
                    var controller = this.ViewContext.Controller as HomeController;

                    if (controller != null)
                    {
                        this.currentAccount = controller.CurrentAccount;
                    }
                }

                return this.currentAccount;
            }
        }

        public StorageUserModel CurrentUser
        {
            get
            {
                if (this.currentAccount == null)
                {
                    var controller = this.ViewContext.Controller as HomeController;

                    if (controller != null)
                    {
                        this.currentUser = controller.CurrentUser;
                    }
                }

                return this.currentUser;
            }
        }

        #endregion
    }
}