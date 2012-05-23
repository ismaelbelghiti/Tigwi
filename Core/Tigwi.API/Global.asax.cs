using System.Web.Mvc;
using System.Web.Routing;

namespace Tigwi.API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // routes to Account

            routes.MapRoute("AccountByName",
                            "account/{action}/name={accountName}/{number}",
                            new
                            {
                                controller = "Account",
                                number = "20" // By default the number is 20. 
                            }
                );

            routes.MapRoute("AccountById",
                            "account/{action}/id={accountId}/{number}",
                            new
                            {
                                controller = "Account",
                                number = "20" 
                            }
                );

            routes.MapRoute("AccountDefault",
                            "account/{action}/{accountName}/{number}",
                            new
                            {
                                controller = "InfoAccount",
                                number = "20" 
                            }
                );

            // routes to List

            routes.MapRoute("List",
                            "list/{action}/{idOfList}/{number}",
                            new
                            {
                                controller = "InfoList",
                                number = "20"
                            }
                );

            // other routes

            routes.MapRoute("Default", "{controller}/{action}");
		}

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}






