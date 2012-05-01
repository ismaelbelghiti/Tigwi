namespace Tigwi.UI
{
    using System.Web.Mvc;

    using Tigwi.UI.Controllers;
    using Tigwi.UI.Models.Storage;

    public abstract class ViewPage<TModel> : WebViewPage<TModel>
    {
        #region Constructors and Destructors

        protected ViewPage()
        {
            var controller = this.ViewContext.Controller as HomeController;

            if (controller == null)
            {
                return;
            }

            this.CurrentUser = controller.CurrentUser;
            this.CurrentAccount = controller.CurrentAccount;
        }

        #endregion

        #region Public Properties

        public StorageAccountModel CurrentAccount { get; set; }

        public StorageUserModel CurrentUser { get; set; }

        #endregion
    }

    public abstract class ViewPage : WebViewPage
    {
        #region Constructors and Destructors

        protected ViewPage()
        {
            var controller = this.ViewContext.Controller as HomeController;

            if (controller == null)
            {
                return;
            }

            this.CurrentUser = controller.CurrentUser;
            this.CurrentAccount = controller.CurrentAccount;
        }

        #endregion

        #region Public Properties

        public StorageAccountModel CurrentAccount { get; set; }

        public StorageUserModel CurrentUser { get; set; }

        #endregion
    }
}