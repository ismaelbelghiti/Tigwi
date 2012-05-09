
// Les routes pour transmettre les informations aux controllers ;

using System.Web.Mvc;
using System.Web.Routing;

namespace Tigwi.API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*
            // general route to ApiController

            routes.MapRoute("CreateUser",
                            "createuser",
                            new
                                {
                                    controller = "Api",
                                    action = "CreateUser"
                                }
                );
            */
            // routes to InfoAccount and ModifyAccount

            routes.MapRoute("InfoAccountByName",
                            "account/{action}/{accountName}/{number}",
                            new
                                {
                                    controller = "InfoAccountByName",
                                    number = "20" // Par défaut le nombre de messages est 20
                                }
                );

            routes.MapRoute("InfoAccountById",
                            "account/{action}/id={accountId}/{number}",
                            new
                            {
                                controller = "InfoAccountById",
                                number = "20" // Par défaut le nombre de messages est 20
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

            routes.MapRoute("InfoList",
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

            routes.MapRoute("InfoUserByLogin", 
                            "user/{action}/{userLogin}/{number}",
                            new
                                {
                                    controller = "InfoUserByLogin",
                                    number = "20"
                                }
                );

            routes.MapRoute("InfoUserById",
                            "user/{action}/id={userId}/{number}",
                            new
                            {
                                controller = "InfoUserById",
                                number = "20"
                            }
                );
		}

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}






