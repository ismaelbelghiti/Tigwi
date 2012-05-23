// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The mvc application entry point.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;

    using Tigwi.Storage.Library;
    using Tigwi.UI.Controllers;
    using Tigwi.UI.Models.Storage;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// The mvc application entry point.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        #region Public Methods and Operators

        /// <summary>
        /// Register global filters.
        /// </summary>
        /// <param name="filters">
        /// The filters to register.
        /// </param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// Register routes.
        /// </summary>
        /// <param name="routes">
        /// The routes to register.
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // routes.IgnoreRoute("favicon.ico");

            routes.MapRoute("Log in", "u/login", new { controller = "User", action = "LogOn" });
            routes.MapRoute("Log out", "u/logout", new { controller = "User", action = "LogOut" });
            routes.MapRoute("Register", "u/register", new { controller = "User", action = "Register" });
            routes.MapRoute("Post", "p/write", new { controller = "Post", action = "Write" });
            // TODO: l/{accountName}/{listName}/edit
            routes.MapRoute("Edit list", "l/{accountName}/{listId}/edit", new { controller = "List", action = "Edit" });
            routes.MapRoute("Create list", "a/{accountName}/lists/new", new { controller = "List", action = "Create" });
            // TODO: l/{accountName}/{listName}/follow
            // TODO: default route to follow user ?
            routes.MapRoute(
                "Follow list", "l/{accountName}/{id}/follow", new { controller = "List", action = "FollowList" });
            routes.MapRoute(
                "Unfollow list", "l/{accountName}/{id}/unfollow", new { controller = "List", action = "UnfollowList" });
            routes.MapRoute(
                "Delete list", "l/{accountName}/{id}/delete", new { controller = "List", action = "Delete" });
            routes.MapRoute("List accounts", "u/accounts", new { controller = "Accounts", action = "List" });
            routes.MapRoute(
                "Make active", "a/{accountName}/select", new { controller = "Account", action = "MakeActive" });
            routes.MapRoute("Create account", "u/accounts/new", new { controller = "Account", action = "Create" });
            routes.MapRoute(
                "Delete account", "a/{accountName}/delete", new { controller = "Account", action = "Delete" });
            routes.MapRoute("Update account", "a/{accountName}/edit", new { controller = "Account", action = "Edit" });
            routes.MapRoute(
                "Following", "a/{accountName}/following", new { controller = "Account", action = "Following" });
            routes.MapRoute("List posts", "{accountName}", new { controller = "Account", action = "Show" });
            routes.MapRoute("Search accounts", "t/search", new { controller = "Account", action = "Search" });

            // Ajax routes
            routes.MapRoute("Validate login", "ajax/user/check", new { controller = "User", action = "ValidateLogin" });
            routes.MapRoute(
                "Validate name", "ajax/account/check", new { controller = "Account", action = "AccountExists" });
            routes.MapRoute("Get list", "ajax/list", new { controller = "List", action = "GetList" });
            routes.MapRoute(
                "List followed?",
                "ajax/list/{listId}/is_followed",
                new { controller = "Account", action = "IsFollowed" });
            routes.MapRoute(
                "Ajax get account", "ajax/account/{accountId}", new { controller = "Account", action = "GetAccount" });
            routes.MapRoute(
                "Ajax autocomplete",
                "ajax/account/{partialAccountName}/autocomplete",
                new { controller = "Account", action = "AutoComplete" });

            routes.MapRoute(
                "Default", 
                // Route name
                "{controller}/{action}/{id}", 
                // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute("NotFound", "{*url}", new { controller = "Error", action = "Http404" });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called on application initialization (every page).
        /// </summary>
        protected void Application_Start()
        {
            IStorage storage = new Storage("sefero", "GU0GjvcPoXKzDFgBSPFbWmCPQrIRHAT6fholbMnxtteY5vQVgYTcWKk/25i/F4m9MFoGHXNf4oYgeAKo+mFO5Q==");

            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new ControllerActivator(storage)));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs ev)
        {
            // Declare variables
            var context = HttpContext.Current;
            var ex = this.Server.GetLastError();
            RouteData routeData;
            RequestContext requestContext;

            // Clear errors
            // context.Response.Clear();
            context.ClearError();

            // Try to get the current context if possible
            var mvcHandler = context.CurrentHandler as MvcHandler;
            if (mvcHandler != null)
            {
                requestContext = mvcHandler.RequestContext;
                routeData = requestContext.RouteData;
            }
            else
            {
                routeData = new RouteData();
                requestContext = new RequestContext(new HttpContextWrapper(context), routeData);
            }

            // Updates route data
            // TODO: 
            routeData.Values["error"] = new HandleErrorInfo(
                ex,
                routeData.Values["controller"] as string ?? context.Request.Url.ToString(),
                routeData.Values["action"] as string ?? context.Request.Url.ToString());
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Http500";

            var httpException = ex as HttpException;
            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }

            var controller = ControllerBuilder.Current.GetControllerFactory().CreateController(requestContext, "Error");
            controller.Execute(requestContext);
        }

        #endregion
    }
}