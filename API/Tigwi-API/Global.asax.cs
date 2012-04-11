
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
			// customer routes

			routes.MapRoute("AccountMessages", 
				"accountmessages/{accountName}/{numberOfMessages}",
				new { controller = "ApiController", 
					action = "AccountMessages", 
					name="", 
					numberOfMessages="20"} // Par défaut le nombre de messages est 20
				);

			routes.MapRoute("AccountsSubscriptions", 
				"accountsubscriptions/{accountName}/{numberOfSubscriptions}",
				new { controller = "ApiController", 
					action = "AccountSubscriptionsList", 
					name="",
					numberOfSubscriptions="20"}
				); 

			routes.MapRoute("Users",
				"accountsubscribers/{accountName}/{numberOfSubscribers}",
				new { controller = "ApiController", 
					action = "AccountSubscribersList", 
					name="",
					numberOfSubscribers="20"}
				);
			
			routes.MapRoute("Write",
				"write",
				new { controller = "ApiController", 
					action = "WritePost"}
				);
		
			routes.MapRoute("SubscribeList",
				"subscribelist",
				new { controller = "ApiController", 
					action = "SuscribeList"}
				);

			// default route
			routes.MapRoute("Default", "{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = ""});
		}

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}






