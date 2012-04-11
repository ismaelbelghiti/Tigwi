
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

			routes.MapRoute("Message", 
				"accountmessages/{accountName}/{numberOfMessages}",
				new { controller = "ApiController", 
					action = "AccountMessages", 
					name="", 
					numberOfMessages="20"} // Par défaut le nombre de messages est 20
				);
            //routes to InfoAccount

            routes.MapRoute("Accounts",
                            "accountsubscriptions/{accountName}/{numberOfSubscriptions}",
                            new
                                {
                                    controller = "ApiController",
                                    action = "AccountSubscriptionsList",
                                    name = "",
                                    numberOfSubscriptions = "20"
                                }
                ); 
            
            routes.MapRoute("InfoAccountMessages",
                            "infoaccount/messages/{accountName}/{numberOfMessages}",
                            new
                                {
                                    controller = "InfoAccountController",
                                    action = "Messages",
                                    accountName = "",
                                    numberOfMessages = "20" // Par défaut le nombre de messages est 20
                                } 
                );

            routes.MapRoute("Users",
                            "accountsubscribers/{accountName}/{numberOfSubscribers}",
                            new
                                {
                                    controller = "ApiController",
                                    action = "AccountSubscribersList",
                                    name = "",
                                    numberOfSubscribers = "20"
                                }
                );

            routes.MapRoute("Message",
                            "write",
                            new
                                {
                                    controller = "ApiController",
                                    action = "WritePost"
                                }
                );

            routes.MapRoute("Accounts",
                            "suscribelist",
                            new
                                {
                                    controller = "ApiController",
                                    action = "SuscribeList"
                                }
                );

            routes.MapRoute("InfoAccountSubscription",
                            "infoaccount/subscriptions/{accountName}/{numberOfSubscriptions}",
                            new
                                {
                                    controller = "InfoAccountController",
                                    action = "SubscriptionsList",
                                    accountName = "",
                                    numberOfSubscriptions = "20"
                                }
                );

            routes.MapRoute("InfoAccountSubscribers",
                            "infoaccount/subscribers/{accountName}/{numberOfSubscribers}",
                            new
                                {
                                    controller = "InfoAcountController",
                                    action = "Subscribers",
                                    accountName = "",
                                    numberOfSubscribers = "20"
                                }
                );

            routes.MapRoute("InfoAccountSubscribedPublicList",
                            "infoaccout/subscribedpubliclists/{accountName}/{numberOfLists}",
                            new
                                {
                                    controller = "InfoAccountController",
                                    action = "SubscribedPublicLists",
                                    accountName = "",
                                    numberOfLists = "20"
                                }
                );

            routes.MapRoute("InfoAccountSubscribedList",
                            "infoaccout/subscribedlists/{accountName}/{numberOfLists}",
                            new
                                {
                                    controller = "InfoAccountController",
                                    action = "SubscribedLists",
                                    accountName = "",
                                    numberOfLists = "20"
                                }
                );

            routes.MapRoute("InfoAccountOwnedPublicList",
                            "infoaccout/ownedpubliclists/{accountName}/{numberOfLists}",
                            new
                                {
                                    controller = "InfoAccountController",
                                    action = "OwnedPublicLists",
                                    accountName = "",
                                    numberOfLists = "20"
                                }
                );

            routes.MapRoute("InfoAccountSubscribedPublicList",
                            "infoaccout/ownedlists/{accountName}/{numberOfLists}",
                            new
                                {
                                    controller = "InfoAccountController",
                                    action = "OwnedLists",
                                    accountName = "",
                                    numberOfLists = "20"
                                }
                );




            //routes to InfoList

            routes.MapRoute("InfoListSubscriptions",
                            "infolist/subscriptions/{idOfList}/{numberOfSubscriptions}",
                            new
                                {
                                    controller = "InfoListController",
                                    action = "Subscriptions",
                                    accountName = "",
                                    numberOfSubscription = "20"
                                }
                );

            routes.MapRoute("InfoListSubscribers",
                            "infolist/subscribers/{idOfList}/{numberOfSubscribers}",
                            new
                                {
                                    controller = "InfoListController",
                                    action = "Subscribers",
                                    idOfList = "",
                                    numberOfSubscribers = "20"
                                }
                );

            routes.MapRoute("InfoListOwner",
                            "infolist/owner/{idOfList}",
                            new
                                {
                                    controller = "InfoListController",
                                    action = "Owner",
                                    idOfList = "",
                                }
                );

            routes.MapRoute("InfoListMessages",
                            "infolist/messages/{idOfList}/{numberOfMessages}",
                            new
                                {
                                    controller = "InfoListController",
                                    action = "Messages",
                                    idOfList = "",
                                    numberOfMessages = "20"
                                }
                );


            //routes to Modify
            routes.MapRoute("ModifyWrite",
                            "modify/write",
                            new
                                {
                                    controller = "ModifyController",
                                    action = "Write"
                                }
                );

            routes.MapRoute("ModifyCreate",
                            "modify/createlist",
                            new
                                {
                                    controller = "ModifyController",
                                    action = "CreateList"
                                }
                );

            routes.MapRoute("ModifyAccountSubscribeList",
                            "modify/accountsuscribelist",
                            new
                                {
                                    controller = "ModifyController",
                                    action = "AccountSuscribeList"
                                }
                );

            routes.MapRoute("ModifyListSubscribeAccount",
                            "modify/ListSubscribeAccount",
                            new
                                {
                                    controller = "ModifyController",
                                    action = "ListSubscribeAccount"
                                }
                );

			// default route
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






