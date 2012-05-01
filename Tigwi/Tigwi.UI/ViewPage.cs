namespace Tigwi.UI
{
    using System;
    using System.Web.Mvc;

    using Tigwi.UI.Controllers;
    using Tigwi.UI.Models;
    using Tigwi.UI.Models.Storage;

    public abstract class ViewPage<TModel> : WebViewPage<TModel>
    {
        #region Constants and Fields

        private IAccountModel currentAccount;

        private StorageUserModel currentUser;

        #endregion

        #region Public Properties

        public IAccountModel CurrentAccount
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
                if (this.currentUser == null)
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

        private IAccountModel currentAccount;

        private IUserModel currentUser;

        #endregion

        #region Public Properties

        public IAccountModel CurrentAccount
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

        public IUserModel CurrentUser
        {
            get
            {
                if (this.currentUser == null)
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