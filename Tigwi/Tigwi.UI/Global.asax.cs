using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tigwi.UI
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Principal;
    using System.Web.ClientServices;
    using System.Web.Security;
    using System.Xml.Serialization;

    using Tigwi.UI.Models;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // TODO: determine a routing scheme and put it in application
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

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs ev)
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return;
            }

            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var serialized = Convert.FromBase64String(ticket.UserData);
            var userData = (new BinaryFormatter()).Deserialize(new MemoryStream(serialized)) as CookieData;

            if (userData == null || userData.UserId == Guid.Empty)
            {
                return;
            }

            this.Context.User =
                new GenericPrincipal(
                    new CustomIdentity(userData.UserId, userData.AccountId, cookie.Name, "Forms"), new string[0]);
            System.Threading.Thread.CurrentPrincipal = this.Context.User;
        }
    }
}