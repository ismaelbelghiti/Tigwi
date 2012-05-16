using System.Web.Mvc;
using System.Web.Routing;

namespace Tigwi.API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // routes to InfoAccount and ModifyAccount

            routes.MapRoute("InfoAccountByName",
                            "account/{action}/name={accountName}/{number}",
                            new
                            {
                                controller = "InfoAccount",
                                number = "20" // By default the number is 20. 
                            }
                );

            routes.MapRoute("InfoAccountById",
                            "account/{action}/id={accountId}/{number}",
                            new
                            {
                                controller = "InfoAccount",
                                number = "20" 
                            }
                );

            routes.MapRoute("InfoAccountDefault",
                            "account/{action}/{accountName}/{number}",
                            new
                            {
                                controller = "InfoAccount",
                                number = "20" 
                            }
                );

            routes.MapRoute("ModifyAccount",
                            "account/{action}",
                            new
                            {
                                controller = "ModifyAccount"
                            }
                );

            // routes to InfoList

            routes.MapRoute("InfoListById",
                            "list/{action}/id={idOfList}/{number}",
                            new
                            {
                                controller = "InfoList",
                                number = "20"
                            }
                );

            routes.MapRoute("InfoListDefault",
                            "list/{action}/{idOfList}/{number}",
                            new
                            {
                                controller = "InfoList",
                                number = "20"
                            }
                );

            routes.MapRoute("ModifyList",
                            "list/{action}",
                            new
                            {
                                controller = "ModifyList"
                            }
                );

            // routes to InfoUser

            routes.MapRoute("InfoUser", 
                            "user/{action}",
                            new
                            {
                                controller = "InfoUser"
                            }
                );
		}

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}






