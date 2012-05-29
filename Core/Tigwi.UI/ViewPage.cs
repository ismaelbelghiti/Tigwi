#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
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
        #region Public Properties

        /// <summary>
        /// Gets the current account, loading it from the controller if needed and possible.
        /// </summary>
        public IAccountModel CurrentAccount
        {
            get
            {
                var controller = this.ViewContext.Controller as HomeController;
                return controller != null ? controller.CurrentAccount : null;
            }
        }

        /// <summary>
        /// Gets the current user, loading it from the controller if needed and possible.
        /// </summary>
        public IUserModel CurrentUser
        {
            get
            {
                var controller = this.ViewContext.Controller as HomeController;
                return controller != null ? controller.CurrentUser : null;
            }
        }

        public IStorageContext Storage
        {
            get
            {
                var controller = this.ViewContext.Controller as HomeController;
                return controller != null ? controller.Storage : null;
            }
        }

        public string Error
        {
            get
            {
                var controller = this.ViewContext.Controller as HomeController;
                return controller != null ? controller.Error : null;
            }
        }

        #endregion
    }

    /// <summary>
    /// The standard view page, for weakly typed views.
    /// </summary>
    public abstract class ViewPage : ViewPage<object>
    {
    }
}