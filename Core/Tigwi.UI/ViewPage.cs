// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewPage.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The custom view page allowing to have statically typed data available in every view.
//   There could be a better way than duplicating the code for strongly and weakly typed views, but it works as it is,
//   so I didn't feel the need to find a better way.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI
{
    using System.Web.Mvc;

    using Tigwi.UI.Controllers;
    using Tigwi.UI.Models;

    /// <summary>
    /// The generics view page (with strongly typed view).
    /// </summary>
    /// <typeparam name="TModel">
    /// The view's strongly typed model.
    /// </typeparam>
    public abstract class ViewPage<TModel> : WebViewPage<TModel>
    {
        #region Constants and Fields

        /// <summary>
        /// The current account.
        /// </summary>
        private IAccountModel currentAccount;

        /// <summary>
        /// The current user.
        /// </summary>
        private IUserModel currentUser;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current account, loading it from the controller if needed and possible.
        /// </summary>
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

        /// <summary>
        /// Gets the current user, loading it from the controller if needed and possible.
        /// </summary>
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

    /// <summary>
    /// The standard view page, for weakly typed views.
    /// </summary>
    public abstract class ViewPage : WebViewPage
    {
        #region Constants and Fields

        /// <summary>
        /// The current account.
        /// </summary>
        private IAccountModel currentAccount;

        /// <summary>
        /// The current user.
        /// </summary>
        private IUserModel currentUser;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current account, loading it from the controller if needed and possible.
        /// </summary>
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

        /// <summary>
        /// Gets the current user, loading it from the controller if needed and possible.
        /// </summary>
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