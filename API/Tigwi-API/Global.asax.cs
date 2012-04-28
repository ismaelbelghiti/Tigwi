
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
                                    action = "",
                                    accountName = @"\s+",
                                    number = "20" // Par défaut le nombre de messages est 20
                                }
                );

            routes.MapRoute("InfoAccountById",
                            "infoaccount/{action}/{accountId}/{number}",
                            new
                            {
                                controller = "InfoAccountById",
                                action = "",
                                accountId = @"\d+",
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
            
            // routes to InfoUser

            routes.MapRoute("InfoUserByLogin", 
                            "infouser/{action}/{userLogin}/{number}",
                            new
                                {
                                    controller = "InfoUserByLogin",
                                    action = "",
                                    userLogin = @"/s+",
                                    number = "20"
                                }
                );

            routes.MapRoute("InfoUserById",
                            "infouser/{action}/{userId}/{number}",
                            new
                            {
                                controller = "InfoUserById",
                                action = "",
                                userId = @"/d+",
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






