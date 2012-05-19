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

    using Tigwi.UI.Controllers;

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

            // TODO: choose a routing scheme
            /*
            routes.MapRoute("Log in", "_login", new { controller = "User", action = "LogOn" });
            routes.MapRoute("Register", "_register", new { controller = "User", action = "Register" });
            routes.MapRoute("Deactivate", "_deactivate", new { controller = "User", action = "Deactivate" });

            routes.MapRoute(
                "User info",
                "{userName}/{controller}/{action}",
                new { controller = "Home", action = "Index" },
                new { userName = @"[a-zA-Z0-9][a-zA-Z0-9_]*", controller = @"Followers|Followed|Tweets" }
            );*/
            routes.MapRoute("UsersAccounts", "{accountName}/Activate", new {controller = "Account", action = "MakeActive"});
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
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs ev)
        {
            var context = this.Context;
            var ex = this.Server.GetLastError();
            context.Response.Clear();
            context.ClearError();
            var httpException = ex as HttpException;

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["exception"] = ex;
            routeData.Values["action"] = "Http500";

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }

        #endregion
    }
}