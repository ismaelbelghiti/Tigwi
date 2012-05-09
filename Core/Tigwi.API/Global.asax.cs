
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

            // routes to InfoAccount and ModifyAccount

            routes.MapRoute("InfoAccountByName",
                            "account/{action}/name={accountName}/{number}",
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

            routes.MapRoute("InfoAccountDefault",
                            "account/{action}/{accountName}/{number}",
                            new
                                {
                                    controller = "InfoAccountByName",
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

            routes.MapRoute("InfoUserByLogin", 
                            "user/{action}/login={userLogin}/{number}",
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

            routes.MapRoute("InfoUserDefault",
                            "user/{action}/{userLogin}/{number}",
                            new
                            {
                                controller = "InfoUserByLogin",
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






