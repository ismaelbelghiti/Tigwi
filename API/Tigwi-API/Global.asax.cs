
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

            // general route to ApiController

            routes.MapRoute("CreateUser",
                            "createuser",
                            new
                                {
                                    controller = "Api",
                                    action = "CreateUser"
                                }
                );

            // routes to InfoAccount

            routes.MapRoute("InfoAccountByName",
                            "infoaccount/{action}/{accountName}/{number}",
                            new
                                {
                                    controller = "InfoAccountByName",
                                    number = "20" // Par défaut le nombre de messages est 20
                                }
                );

            routes.MapRoute("InfoAccountById",
                            "infoaccountbyid/{action}/{accountId}/{number}",
                            new
                            {
                                controller = "InfoAccountById",
                                number = "20" // Par défaut le nombre de messages est 20
                            }
                );

            // routes to InfoList

            routes.MapRoute("InfoList",
                            "infolist/{action}/{idOfList}/{number}",
                            new
                                {
                                    controller = "InfoList",
                                    number = "20"
                                }
                );
            
            // routes to InfoUser

            routes.MapRoute("InfoUserByLogin", 
                            "infouser/{action}/{userLogin}/{number}",
                            new
                                {
                                    controller = "InfoUserByLogin",
                                    number = "20"
                                }
                );

            routes.MapRoute("InfoUserById",
                            "infouserbyid/{action}/{userId}/{number}",
                            new
                            {
                                controller = "InfoUserById",
                                number = "20"
                            }
                );

			// default route used for modify controllers
            routes.MapRoute("Default", "{controller}/{action}");
		}

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}






