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