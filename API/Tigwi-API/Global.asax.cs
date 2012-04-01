
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
				"ApiController/UserTimeline/{name}/{numberOfMessages}",
				new { controller = "ApiController", 
					action = "UserTimeline", 
					name="", 
					numberOfMessages="20"} // Par défaut le nombre de messages est 20
				);

			routes.MapRoute("Users", 
				"ApiController/UserSubscriptionsList/{name}/{numberOfSubscriptions}",
				new { controller = "ApiController", 
					action = "UserSubscriptionsList", 
					name="",
					numberOfSubscriptions="20"}
				); 

			routes.MapRoute("Users",
				"ApiController/UserSubscribersList/{name}/{numberOfSubscribers}",
				new { controller = "ApiController", 
					action = "UserSubscribersList", 
					name="",
					numberOfSubscribers="20"}
				);
			
			routes.MapRoute("Message",
				"ApiController/WritePost/{name}/{message}",
				new { controller = "ApiController", 
					action = "WritePost", 
					name="",
					message=""}
				);
		
			routes.MapRoute("Users",
				"ApiController/Suscribe/{follower}/{followed}",
				new { controller = "ApiController", 
					action = "Suscribe", 
					follower="",
					followed=""}
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






