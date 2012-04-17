
// Les routes pour transmettre les informations aux controllers ;

using System.Web.Mvc;
using System.Web.Routing;

namespace Tigwi_API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //routes to InfoAccount

            routes.MapRoute("InfoAccount",
                            "infoaccount/{action}/{accountName}/{number}",
                            new
                                {
                                    controller = "InfoAccount",
                                    action = "",
                                    accountName = "",
                                    number = "20" // Par défaut le nombre de messages est 20
                                } 
                );

            // routes to InfoList

            routes.MapRoute("InfoList",
                            "infolist/{action}/{idOfList}/{number}",
                            new
                                {
                                    controller = "InfoList",
                                    action = "",
                                    idOfList = "",
                                    number = "20"
                                }
                );
            

			// default route (also used for Modify)
            routes.MapRoute("Default", "{controller}/{action}/{id}",
                            new {controller = "Home", action = "Index", id = ""}
                );
		}

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}






