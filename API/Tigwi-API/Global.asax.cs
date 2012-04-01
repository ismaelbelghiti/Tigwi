
// Les routes pour transmettre les informations aux controllers ;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tigwi_API.Routes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			// customer routes

			routes.MapRoute("Message", 
				"usertimeline/{name}/{numberOfMessages}",
				new { controller = "ApiController", 
					action = "UserTimeline", 
					name="", 
					numberOfMessages="20"} // Par défaut le nombre de messages est 20
				);

			routes.MapRoute("Users", 
				"usersubscriptions/{name}/{numberOfSubscriptions}",
				new { controller = "ApiController", 
					action = "UserSubscriptionsList", 
					name="",
					numberOfSubscriptions="20"}
				); 

			routes.MapRoute("Users",
				"usersubscribers/{name}/{numberOfSubscribers}",
				new { controller = "ApiController", 
					action = "UserSubscribersList", 
					name="",
					numberOfSubscribers="20"}
				);
			
			routes.MapRoute("Message",
				"write",
				new { controller = "ApiController", 
					action = "WritePost"}
				);
		
			routes.MapRoute("Users",
				"suscribe",
				new { controller = "ApiController", 
					action = "Suscribe"}
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






